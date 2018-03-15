@echo off

::协议文件路径, 最后不要跟“\”符号
set SOURCE_FOLDER=.\Proto

set CS_TARGET_PATH=.\CS



::遍历所有文件
for /f "delims=" %%i in ('dir /b "%SOURCE_FOLDER%\*.proto"') do (
	echo protoc.exe --proto_path=%SOURCE_FOLDER% %%~ni.proto --csharp_out=%CS_TARGET_PATH%
	protoc.exe --proto_path=%SOURCE_FOLDER% %%~ni.proto --csharp_out=%CS_TARGET_PATH%
    
)


pause