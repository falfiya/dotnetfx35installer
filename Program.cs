using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;

namespace dotnetfx35installer {
   class Program {
      static string[] files = {
         "microsoft-windows-netfx3-ondemand-package~31bf3856ad364e35~amd64~~.cab",
         "Microsoft-Windows-NetFx3-OnDemand-Package~31bf3856ad364e35~amd64~en-US~.cab"
      };

      static void Main(string[] _) {
         Console.Title = "dotnetfx35installer";
         string outdir = Path.GetTempPath() + @"dotnetfx35installer\";
         Console.WriteLine("Writing temporary installation files to " + outdir);
         Console.Write("Does this look correct? Press 'y' continue: ");
         var confirm = Console.ReadKey().KeyChar;
         Console.Clear();
         if (confirm != 'y') {
            return;
         }
         if (Directory.Exists(outdir)) {
            Directory.Delete(outdir, true);
         }

         Directory.CreateDirectory(outdir);
         var assembly = Assembly.GetExecutingAssembly();
         foreach (var filename in files) {
            var stream = assembly.GetManifestResourceStream($"dotnetfx35installer.{filename}");
            var output = File.Open(outdir + filename, FileMode.CreateNew);
            stream.CopyTo(output);
            stream.Dispose();
            output.Dispose();
         }

         var dism = @"C:\Windows\System32\dism.exe";
         var args = $"/Online /Enable-Feature /FeatureName:NetFX3 /All /LimitAccess /Source:{outdir}";
         Console.WriteLine("Running DISM:");
         Console.WriteLine($"{dism} {args}");
         Process.Start(dism, args).WaitForExit();
         Console.WriteLine("\nFinished... probably...?");
         Console.Title = "yay?";
         Console.WriteLine($"Removing {outdir}");
         Directory.Delete(outdir, true);
         Console.WriteLine("Press any key to exit...");
         Console.ReadKey();
      }
   }
}
