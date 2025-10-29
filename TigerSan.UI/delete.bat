@echo off
setlocal enabledelayedexpansion

:: 定义数组
set "dirs[0]=.vs"
set "dirs[1]=obj"
set "dirs[2]=bin"
set "dirs[3]=x64"
set "dirs[4]=x86"
set "dirs[5]=0 Output"
set count=5

:: 当前目录
for /l %%d in (0,1,%count%) do (
  set "current_dir=!dirs[%%d]!"
  if exist "!current_dir!" (
    echo Deleting root "!current_dir!"
    rmdir /s /q "!current_dir!"
  )
)

:: 子目录
for /d %%i in (*) do (
  :: 遍历数组索引
  for /l %%d in (0,1,%count%) do (
    set "current_dir=!dirs[%%d]!"
    set "target_path=%%i\!current_dir!"
    if exist "!target_path!" (
      echo Deleting "!target_path!"
      rmdir /s /q "!target_path!"
    )
  )
)

endlocal
pause
