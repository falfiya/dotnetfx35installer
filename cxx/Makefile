infile := Main.cxx
outfile := native.exe
nouac := __nouac__
manifest := ../dotnetfx35installer.manifest

G11N := ../proprietary/microsoft-windows-netfx3-ondemand-package~31bf3856ad364e35~amd64~~.cab
L10N := ../proprietary/Microsoft-Windows-NetFx3-OnDemand-Package~31bf3856ad364e35~amd64~en-US~.cab

vcvars = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvars64.bat" &&

native.exe.run: $(outfile)
	$(outfile)

$(nouac): $(infile) res.res
	clang $< res.res -v -o $@

$(outfile): $(nouac)
	$(vcvars) mt -manifest $(manifest) -outputresource:$<;1
	del /f $(outfile)
	MOVE $< $@

# default target
res.res: res.rc $(G11N) $(L10N)
	$(vcvars) rc /V $<

res.rc: res.h

.PHONY: native.exe.run
