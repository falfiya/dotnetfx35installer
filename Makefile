_ := dotnetfx35installer.
clr := cs\clr.exe
x64 := cxx\x64.exe
bclr := bin\$(_)clr.exe
bx64 := bin\$(_)x64.exe

cp = copy /v /y /b

all: $(bclr) $(bx64)

rebuild: clean all

bin:
	mkdir bin

$(bclr): $(clr) bin
	$(cp) $< $@

$(bx64): $(x64) bin
	$(cp) $< $@

$(clr):
	make -C cs

$(x64):
	make -C cxx

clean:
	rd /s /q bin || rem
	make -C cs clean
	make -C cxx clean

.PHONY: all clean
