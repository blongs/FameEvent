using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashInfo : MonoBehaviour {

    public int index;

    public int i;

    public int j;

    public Material[] materials;

    public int delete;

    private Animation ani;
	// Use this for initialization
	void Start ()
    {
        ani = GetComponent<Animation>();
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

    public void PlayAnimation(bool isPlay)
    {
        if (isPlay)
        {
            ani.Play();
        }
        else
        {
            ani.Stop();
            transform.localScale = Vector3.one;
        }
    }
}
