@echo off

echo [generating sql scripts]
scptxfr /s horowitz /d AlchemiMaster /P secret /A /r /H /f sql\Alchemi_structure.sql

echo [deleting bin]
rd bin /s/q

echo.
echo [building Alchemi.sln]
devenv /rebuild release src\Alchemi.sln

echo.
echo [building documentation]
pushd doc
call build.bat
popd
echo.
echo [building Examples.sln]
devenv /rebuild debug examples\Examples.sln

echo.
echo [building Setup.sln]
devenv.exe /rebuild release src\Alchemi.Setup\Setup.sln

echo.
echo [packaging for release]
nant -buildfile:package.nant

echo.
pause