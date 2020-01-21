#include <iostream>
#define _SILENCE_EXPERIMENTAL_FILESYSTEM_DEPRECATION_WARNING
#include <experimental/filesystem>
#include <fstream>
#include <windows.h>
#include <process.h>
#include "res.h"
#define PATH_LENGTH 260
using namespace std;
namespace fs = std::experimental::filesystem;

class EmbeddedFile {
   const char *filename;
   DWORD size;
   HGLOBAL handle;

   public:
   EmbeddedFile(unsigned res_number, const char *filename) {
      this->filename = filename;

      auto res = FindResourceA(nullptr, MAKEINTRESOURCEA(res_number), RT_RCDATA);
      if (res) {
         cerr << "Found resource " << filename << endl;
      } else {
         cerr << "Couldn't find resource " << filename << endl;
         exit(1);
      }

      this->size = SizeofResource(nullptr, res);

      this->handle = LoadResource(nullptr, res);
      if (this->handle) {
         cerr << "Loaded resource " << filename << endl;
      } else {
         cerr << "Couldn't load resource " << filename << endl;
         exit(1);
      }
   }

   void write_file_to(const fs::path dir) {
      auto outname = dir / this->filename;
      cout
         << "Writing "
         << this->size 
         << " bytes from "
         << this->filename
         << " to "
         << outname
         << endl;
      ofstream out(dir / this->filename, std::ios::binary);
      out.write(reinterpret_cast<char *>(LockResource(this->handle)), this->size);
   }
};

int main() {
   const auto temp = fs::temp_directory_path();
   const auto outdir = temp / "dotnetfx35installer";

   cout << "Writing temporary files to " << temp << endl;
   cout << "Does this look correct? Press 'y' to continue: ";
   if (getchar() != 'y') {
      return 0;
   }
   system("cls");
   cout << "Deleted " << fs::remove_all(outdir) << " things\n";
   fs::create_directory(outdir);

   EmbeddedFile g11n(G11N, G11N_NAME);
   EmbeddedFile l10n(L10N, L10N_NAME);
   g11n.write_file_to(outdir);
   l10n.write_file_to(outdir);

   system("cls");
   cout << "Running DISM:\n";
   string source("/Source:");
   source.append(outdir.string());
   const char *dism = "C:\\Windows\\System32\\dism.exe";
   const char *args[] = {
      dism,
      "/Online",
      "/Enable-Feature",
      "/FeatureName:NetFX3",
      "/All",
      "/LimitAccess",
      source.c_str(),
      nullptr,
   };
   auto e = _spawnv(_P_WAIT, dism, args);
   if(e) {
      cerr << "Couldn't spawn dsim.exe\n";
   } else {
      cout << "Finished install\nCleaning up...\n";
      fs::remove_all(outdir);
      cout << "Removed " << outdir << endl;
   }
   cout << "Press any key to exit...\n";
   getchar();
   getchar();
}
