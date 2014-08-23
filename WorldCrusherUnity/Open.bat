@echo off

:: find first filename with *csharp.sln
FOR %%F IN (*csharp.sln) DO (
 set filename=%%F
 goto filenamefound
)

:: no csharp solution found, skip to opening unity
goto openunity

:filenamefound
echo "%filename%"
:: open csharp solution with standard program
start %filename%

:openunity
:: open Unity Editor with the project in this path
IF EXIST "C:\Program Files (x86)\Unity\Editor\Unity.exe" (
	start "" "C:\Program Files (x86)\Unity\Editor\Unity.exe" -projectPath %~dp0
)
IF EXIST "D:\Development\Unity\Editor\Unity.exe" (
	start D:\Development\Unity\Editor\Unity.exe -projectPath %~dp0
)

exit