using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 此类是在UI上在封装一层
/// </summary>
public class UIBehaviour : MonoBehaviour {

    void Awake()
    {
        UIManager.Instance.RegistGameObject(name,gameObject);
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    
    public void AddButtonListener(UnityAction action)
    {
        if (action != null)
        {
            Button btn = transform.GetComponent<Button>();
            btn.onClick.AddListener(action);
        }
    }

    public void RemoveButtonListener(UnityAction action)
    {
        if (action != null)
        {
            Button btn = transform.GetComponent<Button>();
            btn.onClick.RemoveListener(action);
        }
    }
}
