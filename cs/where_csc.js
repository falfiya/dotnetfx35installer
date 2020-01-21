const fs = require("fs");


const rosyln_installs = (
   require("fs")
      .readdirSync(".nuget")
      .filter(dirent => dirent.startsWith("Microsoft.Net.Compilers"))
);
if (rosyln_installs.length < 1) {
   process.stderr.write("Rosyln compilers are not installed!");
   process.exit(1);
}
process.stdout.write(`.nuget\\${rosyln_installs[0]}\\tools\\csc.exe`);
