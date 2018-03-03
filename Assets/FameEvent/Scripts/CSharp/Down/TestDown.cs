using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDown : MonoBehaviour {

    string path = "http://127.0.0.1/Resourses/"+IPathTools.GetPlatformFolderName()+".zip";
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DownResource()
    {
        UpdateHelper.Instance.DownResource(path,null);
    }
}
