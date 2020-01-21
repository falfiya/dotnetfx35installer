csc := $(shell node where_csc.js)
entry := Main.cs
others := AssemblyInfo.cs
outfile := .clr.exe

$(outfile): $(entry) $(others)
	$(csc) $^ /out:$@
