using System.IO;
using System.Reflection;
using System.Diagnostics;
using static System.Console;
class _ {
   static string[] files = {
      "microsoft-windows-netfx3-ondemand-package~31bf3856ad364e35~amd64~~.cab",
      "Microsoft-Windows-NetFx3-OnDemand-Package~31bf3856ad364e35~amd64~en-US~.cab"
   };

   static void Main(string[] _) {
      Title = "dotnetfx35installer.clr";

      var outdir = Path.GetTempPath() + @"dotnetfx35installer\";
      Write(
         $"Writing temporary files to {outdir}\n"
         + "\nDoes this look correct? Press 'y' continue: "
      );
      if (ReadKey().KeyChar != 'y') {
         return;
      }
      Clear();

      Title = "Installing...";

      if (Directory.Exists(outdir)) {
         Directory.Delete(outdir, true);
      }
      Directory.CreateDirectory(outdir);

      var assembly = Assembly.GetExecutingAssembly();
      WriteLine("\nGetting resources:");
      var names = assembly.GetManifestResourceNames();
      for (var i = 0; i < names.Length; ++i) {
         var name = names[i];
         WriteLine($"{i}: {name}");
         var res = assembly.GetManifestResourceStream(name);
         var file = File.Open(outdir + name, FileMode.CreateNew);
         res.CopyTo(file);
         res.Dispose();
         file.Dispose();
         WriteLine($"{i}: Done");
      }

      var dism = @"C:\Windows\System32\dism.exe";
      var args = $"/Online /Enable-Feature /FeatureName:NetFX3 /All /LimitAccess /Source:{outdir}";
      WriteLine($"{dism} {args}");
      WriteLine("Please be patient!");
      Process.Start(dism, args).WaitForExit();

      Write($"Finished install\nCleaning Up...\n");
      Directory.Delete(outdir, true);

      Title = "Press any key to exit...";
      WriteLine("Press any key to exit...");
      ReadKey();
   }
}
