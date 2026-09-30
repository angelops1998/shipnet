@echo off
title ShipNet - Detener Sistema UPDS
color 0c

echo ===================================================
echo           DETENIENDO SISTEMA UPDS SHIPNET
echo ===================================================
echo.
echo Deteniendo procesos dotnet (ShipNetApi y ShipNetMvc)...
taskkill /f /im dotnet.exe 2>nul
taskkill /f /im ShipNetMvc.exe 2>nul
taskkill /f /im ShipNetApi.exe 2>nul

echo.
echo Todos los servidores de ShipNet han sido detenidos.
echo.
pause
