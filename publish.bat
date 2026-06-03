@echo off
cd /d "%~dp0"

echo Publishing to bin folder...
dotnet publish TBank_GetUserId.csproj -c Release -o bin
if errorlevel 1 (
    echo.
    echo BUILD FAILED!
    pause
    exit /b 1
)

echo.
echo Done: "%~dp0bin\TBank_GetUserId.exe"
