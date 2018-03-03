using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Threading;

public class UpdateHelper : MonoBehaviour
{
    /// <summary>
    /// 解压的支线程
    /// </summary>
    internal class ThreadUnZip
    {
        string filePath;

        public ThreadUnZip(string _filePath)
        {
            filePath = _filePath;
        }

        public void UnZip()
        {
            UnZipFiles unZipFiles = new UnZipFiles(filePath);
            unZipFiles.UnZipFile();
        }

    }

    static UpdateHelper _instance;

    /// <summary>
    /// 当前正在下载的文件地址
    /// </summary>
    string mDownloadingUrl = "";

    public static UpdateHelper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("UpdateHelper");
                _instance = go.AddComponent<UpdateHelper>();
            }
            return _instance;
        }

    }

    /// <summary>
    /// 下载文件对外接口
    /// </summary>
    /// <param name="_path"></param>
    /// <param name="_callBack"></param>
    public void DownResource(string _path, Action<WWW> _callBack)
    {
        Debug.Log("UpdateHelper::UpdateResource...");
        if (IsDownLoadingFile(_path))
        {
            return;
        }
        StartCoroutine(DownLoad(_path, delegate (WWW www)
        {
            string fileName = _path.Split('/')[_path.Split('/').Length - 1];
            if (SaveDataToLocal(fileName, www.bytes))
            {
                string dataPath = IPathTools.DownLoadAssetBundlePath.Replace("file://", "") + Path.DirectorySeparatorChar + fileName;
                ThreadUnZip threadUnZip = new ThreadUnZip(dataPath);
                Thread thread = new Thread(new ThreadStart(threadUnZip.UnZip));
                thread.Start();
            }
        }
        ));
    }

    /// <summary>
    /// 保存文件到本地
    /// </summary>
    /// <param name="_fileName">文件名字</param>
    /// <param name="_data">下载好的二进制流</param>
    /// <returns></returns>
    private static bool SaveDataToLocal(string _fileName, byte[] _data)
    {
        string localFileDir = IPathTools.DownLoadAssetBundlePath.Replace("file://", "");// + Path.DirectorySeparatorChar + _filePath;
        Debug.Log("localFileDir = " + localFileDir);
        try
        {

            string dataPath = localFileDir + Path.DirectorySeparatorChar + _fileName;
            Debug.Log("dataPath = " + dataPath);
            if (!Directory.Exists(localFileDir))
            {
                Directory.CreateDirectory(localFileDir);
            }
            BinaryWriter writer = new BinaryWriter(File.Create(dataPath));
            writer.Write(_data);
            writer.Close();
        }
        catch (Exception e)
        {
            Debug.LogError(e.ToString());
            return false;
        }

        return true;
    }


    /// <summary>
    /// 下载文件的协程
    /// </summary>
    /// <param name="_path"></param>
    /// <param name="_finishCallBack"></param>
    /// <returns></returns>
    IEnumerator DownLoad(string _path, Action<WWW> _finishCallBack)
    {
        Debug.Log("UpdateHelper::DownLoad -> _path=" + _path);
        SetDownLoadingFile(_path);

        WWW www = new WWW(_path);
        yield return www;
        if (www.isDone && string.IsNullOrEmpty(www.error))
        {
            SetDownLoadingFile("");
            if (_finishCallBack != null)
            {
                _finishCallBack(www);
            }
        }
        else
        {
            Debug.LogError("WWW DownLoad Failed,The Path is :" + _path + ",error is :" + www.error);
        }
        www.Dispose();
    }


    private bool IsDownLoadingFile(string _fileUrl)
    {
        if (string.IsNullOrEmpty(_fileUrl))
        {
            return false;
        }

        return _fileUrl.Equals(mDownloadingUrl);
    }

    /// <summary>
    /// 设置当前正在下载的文件
    /// </summary>
    /// <param name="_fileUrl"></param>
    private void SetDownLoadingFile(string _fileUrl)
    {
        mDownloadingUrl = _fileUrl;
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
