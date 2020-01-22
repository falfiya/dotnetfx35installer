include ../vars.mk

outfile := clr.exe

csc := $(shell node where_csc.js)
args := \
/target:exe \
/win32manifest:$(manifest) \
/resource:$(G11N) \
/resource:$(L10N) \
/optimize+ \
/warnaserror+

$(outfile): *.cs $(G11N) $(L10N)
	$(csc) *.cs $(args) /out:$@

clean:
	del /f clr.exe
	rd /s /q bin || rem
	rd /s /q obj || rem

.PHONY: clean
