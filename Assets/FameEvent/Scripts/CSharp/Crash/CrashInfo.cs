using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashInfo : MonoBehaviour {

    public int index;

    public int i;

    public int j;

    public Material[] materials;

    public int delete;
	// Use this for initialization
	void Start ()
    {
        index = Random.Range(0,4);
        GetComponent<MeshRenderer>().material = materials[index];
        delete = 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetIndex(int _index)
    {
        /*
        if (index == _index)
        {
            return;
        }
        else
        */
        {
            index = _index;
            GetComponent<MeshRenderer>().material = materials[index];
        }
    }

    public void SetDelete(int _delete)
    {
        delete = _delete;
    }
}
