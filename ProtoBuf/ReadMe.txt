参考资料：https://www.jianshu.com/p/b135676dbe8d


protoc-3.5.1   文件夹是proto文件转化CS文件的文件夹
执行protoc-3.5.1\bin\Run.bat文件可将 protoc-3.5.1\bin\Proto文件夹中的proto文件转化为相应的CS文件，存放在protoc-3.5.1\bin\CS文件夹中


csharp文件夹是用来生成工程需要用的protol.dll
1.打开csharp/src/Google.Protobuf.sln
2.生成Google.Protobuf.dll
3.在csharp\src\Google.Protobuf\bin\Debug\net45\Google.Protobuf.dll
  或csharp\src\Google.Protobuf\bin\Debug\netstandard1.0\Google.Protobuf.dll 中找到适配的dll

