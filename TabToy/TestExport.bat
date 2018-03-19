@echo off

set SOURCE_FOLDER=.\Tables\TableTest\

set CS_TARGET_PATH=..\.\Assets\FameEvent\Scripts\CSharp\TabToy\TabScipts\

set CS_DATABIN_PATH=..\.\Assets\StreamingAssets\DataBin\

echo tabtoy.exe --mode=v2 --csharp_out=%CS_TARGET_PATH%TableTest.cs --binary_out=%CS_DATABIN_PATH%TableTest.bin --lan=zh_cn %SOURCE_FOLDER%Blongs.xlsx %SOURCE_FOLDER%Sample.xlsx

@tabtoy.exe ^
--mode=v2 ^
--csharp_out=%CS_TARGET_PATH%TableTest.cs ^
--binary_out=%CS_DATABIN_PATH%TableTest.bin ^
--lan=zh_cn ^
%SOURCE_FOLDER%Blongs.xlsx ^
%SOURCE_FOLDER%Sample.xlsx
@IF %ERRORLEVEL% NEQ 0 pause

pause

