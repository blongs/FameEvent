using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

/// <summary>
/// 解压工具类
/// </summary>
public class UnZipFiles
{
    private string filePath = "";


    public UnZipFiles(string _filePath)
    {
        filePath = _filePath;
    }

    public void UnZipFile()
    {
        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        using (ZipInputStream s = new ZipInputStream(File.OpenRead(filePath)))
        {
            try
            {
                ZipEntry zipEntry;
                string unZipFileDiretory = "";
                unZipFileDiretory = filePath.Replace(".zip","");
                Debug.Log("unZipFileDiretory = " + unZipFileDiretory);
                while ((zipEntry = s.GetNextEntry()) != null)
                {

                    string directoryName = Path.GetDirectoryName(unZipFileDiretory);
                    if (zipEntry.Name != string.Empty)
                    {
                        string pathName = Path.GetDirectoryName(zipEntry.Name);
                        string fileName = Path.GetFileName(zipEntry.Name);

                        pathName = pathName.Replace(":","$");
                        directoryName = directoryName + Path.DirectorySeparatorChar + pathName;

                        Debug.Log("directoryName = "+ directoryName);

                        Directory.CreateDirectory(directoryName);
                        if (fileName != string.Empty)
                        {
                            using (FileStream sw = File.Create(directoryName + Path.DirectorySeparatorChar + fileName))
                            {
                                int size = 1024;
                                byte[] data = new byte[size];
                                while (true)
                                {
                                    size = s.Read(data,0,data.Length);
                                    if (size > 0)
                                    {
                                        sw.Write(data, 0, size);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
        File.Delete(filePath);
    }
}
