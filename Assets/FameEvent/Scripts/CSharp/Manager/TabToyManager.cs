using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TabToyEvent
{
    TableTestBengain = ManagerId.TabToyManager + 1,
    TableTestGetSample,
    TableTestBackSample,
    TableTestGetBlongs,
    TableTestBackBlongs,
    TableTestEnd = ManagerId.TabToyManager + FrameTools.TableSpan,

    TableCharactorBengain = ManagerId.TabToyManager + FrameTools.TableSpan * 1 + 1,
    TableCharactorGet,
    TableCharactorBack,
    TableCharactorEnd = ManagerId.TabToyManager + FrameTools.TableSpan * 2,
}

public class TabToyManager : ManagerBase
{

    public static TabToyManager Instance = null;
    private TableTestLoader tableTestLoader;
    private TableCharactorLoader charactorLoader;

    private void Awake()
    {
        Instance = this;
        InitTableLoaders();
    }

    private void InitTableLoaders()
    {
        if (tableTestLoader == null)
        {
            GameObject go = new GameObject("TableTestLoader");
            go.AddComponent<TableTestLoader>();
            go.transform.parent = transform;

        }
        if (charactorLoader == null)
        {
            GameObject go = new GameObject("TableCharactorLoader");
            go.AddComponent<TableCharactorLoader>();
            go.transform.parent = transform;
        }
    }

    public void SendMsg(MsgBase msg)
    {
        if (msg.GetManager() == ManagerId.TabToyManager)
        {
            ProcessEvent(msg);
        }
        else
        {
            MsgCenter.Instance.SendToMsg(msg);
        }
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
