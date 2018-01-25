using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

public class AssetbundleEditor
{


    [MenuItem("Tools/MarkAssetBundle")]
    public static void MarkAssetBundle()
    {
        AssetDatabase.RemoveUnusedAssetBundleNames();
        //先删除之前的ab包资源
        ClearDir(IPathTools.GetAssetBundlePath());
        string path = Application.dataPath + "/Art/Scences";
        DirectoryInfo dir = new DirectoryInfo(path);

        FileSystemInfo[] fileInfo = dir.GetFileSystemInfos();

        for (int i = 0; i < fileInfo.Length; i++)
        {
            FileSystemInfo tmpFile = fileInfo[i];
            if (tmpFile is DirectoryInfo)
            {
                // string tmpPath = Path.Combine(path, tmpFile.Name);
                string tmpPath = path + "/" + tmpFile.Name;
                ScencesOverview(tmpPath, tmpFile.Name);
            }
        }
        AssetDatabase.Refresh();
    }

    public static void ClearDir(string srcPath)
    {
        if (Directory.Exists(srcPath))
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(srcPath);
                FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录
                foreach (FileSystemInfo i in fileinfo)
                {
                    if (i is DirectoryInfo)            //判断是否文件夹
                    {
                        DirectoryInfo subdir = new DirectoryInfo(i.FullName);
                        subdir.Delete(true);          //删除子目录和文件
                    }
                    else
                    {
                        File.Delete(i.FullName);      //删除指定文件
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        else
        {
            Debug.LogError("清空"+ srcPath+"文件夹失败,该文件夹不存在，现在已经创建，如果不对，请删除");
            if (!Directory.Exists(srcPath))
            {
                Directory.CreateDirectory(srcPath);
            }
        }
       
    }


    [MenuItem("Tools/BuideAssetBundle")]
    public static void BuideAssetBundle()
    {
        string outPath = IPathTools.GetAssetBundlePath();// + "/AssetBundle";
        BuildPipeline.BuildAssetBundles(outPath, 0, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    //怼整个场景遍历
    public static void ScencesOverview(string scencePath,string filePath)
    {
        string textFileName = "Record.txt";
        string tmpPath = scencePath + textFileName;

        FileStream fs = new FileStream(tmpPath,FileMode.OpenOrCreate);

        StreamWriter bw = new StreamWriter(fs);
        //存储对应关系
        Dictionary<string, string> readDict = new Dictionary<string, string>();
        ChangerHead(scencePath, readDict);

        bw.WriteLine(readDict.Count);
        foreach (string key in readDict.Keys)
        {
            bw.Write(key);
            bw.Write(" ");
            bw.Write(readDict[key]);
            bw.Write("\n");
        }

        bw.Close();
        fs.Close();
        System.IO.File.Copy(tmpPath,IPathTools.GetAssetBundlePath()+"/"+filePath+textFileName);
    }

    //截取 相对路径
    public static void ChangerHead(string fullPath,Dictionary<string,string> theWriter)
    {
        int tmpCount = fullPath.IndexOf("Asset");

        int tmpLength = fullPath.Length;

        string replacePath = fullPath.Substring(tmpCount,tmpLength - tmpCount);

        DirectoryInfo dir = new DirectoryInfo(fullPath);
        if (dir != null)
        {
           ListFiles(dir,replacePath,theWriter);
        }
        else
        {
            Debug.LogError("this path in not exitance");
        }
    }


    public static void ListFiles(FileSystemInfo info, string replacePath,Dictionary<string,string> theWriter)
    {
        if (info.Exists)
        {
            DirectoryInfo dir = info as DirectoryInfo;
            FileSystemInfo[] files = dir.GetFileSystemInfos();
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //这里是文件的操作
                if (file != null)
                {
                    ChangerMark(file,replacePath,theWriter);
                }
                else//这里是目录
                {
                    ListFiles(files[i],replacePath,theWriter);
                }
            }
        }
        else
        {
            Debug.LogError("this info in not exitance");
            return;
        }
    }

    public static string FixedPath(string path)
    {
        path = path.Replace("\\","/");
        return path;
    }

   
    //改变物体的tag
    public static void ChangerMark(FileInfo tmpFile, string replacePath, Dictionary<string, string> theWriter)
    {
        if (tmpFile.Extension == ".meta")
        {
            return;
        }
        string markStr = GetBundlePath(tmpFile,replacePath);
        ChangeAssetMark(tmpFile,markStr,theWriter);
    }

    //计算Mark标记值
    public static string GetBundlePath(FileInfo file, string replacePath)
    {
        string tmpPath = file.FullName;
        tmpPath = FixedPath(tmpPath);

        int assetCount = tmpPath.IndexOf(replacePath);
        assetCount += replacePath.Length + 1;
        int nameCount = tmpPath.LastIndexOf(file.Name);
        int tmpCount = replacePath.LastIndexOf("/");
        string scenceHead = replacePath.Substring(tmpCount + 1, replacePath.Length - tmpCount - 1);
        int tmpLength = nameCount - assetCount;
        if (tmpLength > 0)
        {
            string subString = tmpPath.Substring(assetCount, tmpPath.Length - assetCount);
            string[] result = subString.Split("/".ToCharArray());
            return scenceHead + "/" + result[0];
            /*
            string tmp = "";
            for (int i = 0; i < result.Length - 1; i++)
            {
                tmp = tmp + "/" + result[i];
            }
            return scenceHead + tmp;
            */
        }
        else
        {
            return scenceHead;
        }
    }

    public static void ChangeAssetMark(FileInfo tmpFile, string markStr, Dictionary<string, string> theWriter)
    {
        string fullPath = tmpFile.FullName;
        int assetCount = fullPath.IndexOf("Assets");
        string assetPath = fullPath.Substring(assetCount,fullPath.Length - assetCount);
        AssetImporter importer = AssetImporter.GetAtPath(assetPath);
        importer.assetBundleName = markStr;
        if (tmpFile.Extension == ".unity")
        {
            importer.assetBundleVariant = "u3d";

        }
        else
        {
            importer.assetBundleVariant = "ld";
        }
        string modleName = "";
        string[] subMark = markStr.Split("/".ToCharArray());
        if (subMark.Length > 1)
        {
            modleName = subMark[1];
        }
        else
        {
            modleName = markStr;
        }
        //小写加上后缀
        string modelPath = markStr.ToLower() + "." + importer.assetBundleVariant;
        if (!theWriter.ContainsKey(modleName))
        {
            theWriter.Add(modleName, modelPath);
           // ChandeAssetMark(tmpFile,markStr,theWriter);
        }
    }

}
