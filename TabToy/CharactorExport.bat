@echo off

set SOURCE_FOLDER=.\Tables\Charactor\

set CS_TARGET_PATH=..\.\Assets\FameEvent\Scripts\CSharp\TabToy\TabScipts\

set CS_DATABIN_PATH=..\.\Assets\StreamingAssets\DataBin\



echo tabtoy.exe --mode=v2 --csharp_out=%CS_TARGET_PATH%Charactor.cs --binary_out=%CS_DATABIN_PATH%Charactor.bin --lan=zh_cn %SOURCE_FOLDER%Charactor.xlsx

tabtoy.exe ^
--mode=v2 ^
--csharp_out=%CS_TARGET_PATH%Charactor.cs ^
--binary_out=%CS_DATABIN_PATH%Charactor.bin ^
--lan=zh_cn ^
%SOURCE_FOLDER%Charactor.xlsx 

@IF %ERRORLEVEL% NEQ 0 pause

pause

