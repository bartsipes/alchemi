@echo off

rem --- userguide ---

rd userguide /s/q
doxygen userguide.doxygen.cfg
xcopy userguide_img userguide\img\ /Y

rem pause
