using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TimerManager : MonoBehaviour
{


    class TimerInfo
    {
        public float resetCountDownTime;

        public bool repeat;

        public bool IgnoreTimeScale;

        public float interval;

        public Action callBack;


        public TimerInfo(float _resetCountDownTime, Action _callBack, bool _repeat, float _interval, bool _IgnoreTimeScale)
        {
            resetCountDownTime = _resetCountDownTime;
            callBack = _callBack;
            repeat = _repeat;
            interval = _interval;
            IgnoreTimeScale = _IgnoreTimeScale;
        }
    }

    static TimerManager _instance = null;

    public static TimerManager Instance
    {

        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("TimerManager");
                _instance = go.AddComponent<TimerManager>();
            }
            return _instance;
        }
    }

    public static bool Exist
    {
        get
        {
            return _instance != null;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    uint _unique = 0;
    List<uint> AllTimerIds = new List<uint>();
    Dictionary<uint, TimerInfo> AllTimerInfos = new Dictionary<uint, TimerInfo>();


    public uint StartTimer(float _countdown, Action _callBack, bool _repeat = false, float _interval = 0, bool _ignoreTimeScale = false)
    {
        _unique++;
        TimerInfo timer = null;
        if (_countdown <= 0)
        {
            if (_callBack != null)
            {
                _callBack();
            }
            if (_repeat)
            {
                timer = new TimerInfo(_interval, _callBack, _repeat, _interval, _ignoreTimeScale);
            }
        }
        else
        {
            timer = new TimerInfo(_countdown, _callBack, _repeat, _interval, _ignoreTimeScale);
        }

        if (timer != null)
        {
            AllTimerIds.Add(_unique);
            AllTimerInfos.Add(_unique, timer);
        }
        return _unique;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (AllTimerIds.Count > 0)
        {
            float deltaTime = Time.deltaTime;
            float realDalta = Time.unscaledDeltaTime;
            for (int i = AllTimerIds.Count - 1; i >= 0; i--)
            {
                uint id = AllTimerIds[i];
                if (AllTimerInfos.ContainsKey(id))
                {
                    TimerInfo ti = AllTimerInfos[id];
                    if (ti.IgnoreTimeScale)
                    {
                        ti.resetCountDownTime = ti.resetCountDownTime - realDalta;
                    }
                    else
                    {
                        ti.resetCountDownTime = ti.resetCountDownTime = -realDalta;
                    }

                    if (ti.resetCountDownTime <= 0)
                    {
                        if (ti.callBack != null)
                        {
                            ti.callBack();
                        }
                        if (ti.repeat)
                        {
                            ti.resetCountDownTime = ti.interval;
                        }
                        else
                        {
                            AllTimerIds.Remove(id);
                            AllTimerInfos.Remove(id);
                        }
                    }
                }
            }
        }
    }

    public void RemoveTimer(uint _id)
    {
        if (AllTimerInfos.ContainsKey(_id))
        {
            AllTimerInfos.Remove(_id);
            AllTimerIds.Remove(_id);
        }
    }

    public void OnDestroy()
    {
        _instance = null;
    }
}
