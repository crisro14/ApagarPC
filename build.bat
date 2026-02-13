@echo off
setlocal

:: Check for standard .NET 4.0 locations
set "CSC=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
if exist "%CSC%" goto :Found

set "CSC=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe"
if exist "%CSC%" goto :Found

:: If not found, try searching (slower)
echo Buscando csc.exe...
for /r "C:\Windows\Microsoft.NET\Framework64" %%f in (csc.exe) do (
    set "CSC=%%f"
    goto :Found
)

:Found
if not defined CSC (
    echo No se encontro csc.exe.
    pause
    exit /b 1
)

echo Usando compilador: %CSC%
echo Compilando ApagarPC...
"%CSC%" /nologo /target:winexe /out:ApagarPC.exe ApagarPC.cs
if %errorlevel% neq 0 (
    echo.
    echo ERRORES DE COMPILACION:
    echo -----------------------
    pause
    exit /b %errorlevel%
)

echo.
echo ========================================
echo   COMPILACION EXITOSA
echo   ApagarPC.exe ha sido creado.
echo ========================================
echo.
pause
