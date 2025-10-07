@echo off
setlocal enabledelayedexpansion

rem 检查当前目录下是否存在名为 “.vs” 的文件夹
if exist ".vs" (
  echo Deleting .vs
  rmdir /s /q ".vs"
)

rem 检查当前目录下是否存在名为 “0 Output” 的文件夹
if exist "0 Output" (
  echo Deleting .vs
  rmdir /s /q "0 Output"
)

rem 遍历当前目录及其子目录
for /d /r %%i in (*) do (
  rem 检查目录名是否为 “obj” 或 “bin”
  if /i "%%~nxi"=="obj" (
    echo Deleting %%i
    rmdir /s /q "%%i"
  )
  if /i "%%~nxi"=="bin" (
    echo Deleting %%i
    rmdir /s /q "%%i"
  )
)

endlocal

pause
