include ../vars.mk

infile := Main.cxx
outfile := x64.exe

vcvars = "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\VC\Auxiliary\Build\vcvars64.bat" &&

$(outfile): $(infile) res.res
	clang $< res.res -Wall -O2 -o $@
	$(vcvars) mt -manifest $(manifest) -outputresource:$@;1

res.res: res.rc $(G11N) $(L10N)
	$(vcvars) rc /V $<

res.rc: res.h

clean:
	del /f $(outfile) res.res

.PHONY: clean
