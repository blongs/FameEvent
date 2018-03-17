: 输出C#源码,二进制(例子中供C#读取), lua表, json格式
: 适用于csharp, golang, lua例子
tabtoy.exe ^
--mode=v2 ^
--csharp_out=..\.\Assets\FameEvent\Scripts\CSharp\TabToy\Config.cs ^
--binary_out=..\.\Assets\StreamingAssets\DataBin\Config.bin ^
--lan=zh_cn ^
.\XLSXS\Globals.xlsx ^
.\XLSXS\Sample.xlsx

@IF %ERRORLEVEL% NEQ 0 pause