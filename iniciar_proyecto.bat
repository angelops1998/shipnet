@echo off
title ShipNet - Iniciar Sistema UPDS
color 0b

echo ===================================================
echo           INICIANDO SISTEMA UPDS SHIPNET
echo ===================================================
echo.

:: 1. Verificar e iniciar MySQL (XAMPP) si no esta corriendo
echo [1/3] Verificando servicio de Base de Datos MySQL...
netstat -ano | findstr :3306 >nul
if %errorlevel% neq 0 (
    echo Iniciando MySQL desde XAMPP...
    start "" /B "C:\xampp\mysql\bin\mysqld.exe" --defaults-file="C:\xampp\mysql\bin\my.ini" --console
    timeout /t 3 >nul
) else (
    echo MySQL ya esta activo en el puerto 3306.
)
echo.

:: 2. Iniciar Backend ShipNetApi en nueva ventana (Puerto 5220)
echo [2/3] Iniciando API REST Backend (http://localhost:5220)...
start "ShipNet Backend API (Puerto 5220)" cmd /k "cd /d %~dp0ShipNetApi && dotnet run --urls http://localhost:5220"
timeout /t 3 >nul
echo.

:: 3. Iniciar Frontend ShipNetMvc en nueva ventana (Puerto 5176 para Red Local y PC)
echo [3/3] Iniciando Frontend Web MVC (http://0.0.0.0:5176)...
start "ShipNet Frontend MVC (Puerto 5176)" cmd /k "cd /d %~dp0ShipNetMvc && dotnet run --urls http://0.0.0.0:5176"
timeout /t 3 >nul
echo.

echo ===================================================
echo          TODO EL SISTEMA ESTA EN EJECUCION
echo ===================================================
echo.
echo - Web Principal (Tu PC): http://localhost:5176
echo - API REST Backend:      http://localhost:5220
echo.
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /c:"IPv4" /c:"Direcci"') do (
    echo - Para entrar desde tu CELULAR / OTRA PC: http:%%a:5176
)
echo.
echo ===================================================
echo Abriendo navegador en la pagina principal...
start http://localhost:5176
echo.
echo (Puedes cerrar esta ventana cuando quieras. Los servidores continuaran activos en sus respectivas ventanas).
pause
