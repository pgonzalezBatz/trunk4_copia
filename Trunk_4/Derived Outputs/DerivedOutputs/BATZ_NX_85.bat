@echo off

rem set UGII_BASE_DIR=C:\CADSoft\NX\NX 9.0
set UGII_BASE_DIR=\\hpnas2\CATIAV5CFG\UGS\nx-11.0.0.mp01
rem set UGII_BASE_DIR=\\hpnas2\CATIAV5CFG\UGS\NX.8.5.1.3.mp03
set UGII_ROOT_DIR=%UGII_BASE_DIR%\ugii
set UGS_LICENSE_SERVER=28000@lobantzo
set SPLM_LICENSE_SERVER=28000@lobantzo
rem set UGS_LICENSE_BUNDLE=NX13100N
set UGS_LICENSE_BUNDLE=GMS4050
set ugBat="%UGII_ROOT_DIR%\"ugii.bat
call %ugBat%
rem start "Title" "%UGII_ROOT_DIR%\"ugraf.exe %*
