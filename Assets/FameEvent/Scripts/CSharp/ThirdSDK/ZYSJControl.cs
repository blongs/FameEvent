using UnityEngine;
using System.Collections;

public class ZYSJControl : MonoBehaviour
{
    Rect windowRect = new Rect(20, 20, Screen.width-40, Screen.height-40);
    GUI.WindowFunction windowFunction;
    public GUISkin guiSkin;
    string str = "MainCamera";
    // Use this for initialization
    void Start()
    {
        windowFunction = DoMyWindow;
        AddSDKListeners();
    }


    void AddSDKListeners()
    {
        AndroidChannelSDKMgr.loginEvent += OnSDKLogin;
        AndroidChannelSDKMgr.logoutEvent += OnSDKLogout;
        AndroidChannelSDKMgr.payEvent += OnSDKPay;
        AndroidChannelSDKMgr.switchUserEvent += OnSDKSwitchUser;
    }
    void OnSDKLogin(bool isSuccess)
    {
        Debug.Log("isSuccess = " + isSuccess);
        str = "isSuccess =" + isSuccess+ChannelUserInfo.Instance.SDKUser.ToString();
    }
    void OnSDKLogout()
    {
        Debug.Log("Boot::OnSDKLogout...");
       
    }
    void OnSDKPay(bool isSuccess, string orderNo)
    {
        Debug.Log("Boot::OnSDKPay-> isSuccess = " + isSuccess + ", orderNo = " + orderNo);
    }
    void OnSDKSwitchUser(bool needLogout)
    {
        Debug.Log("Boot::OnSDKSwitchUser...");
    }
    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (guiSkin)
        {
            GUI.skin = guiSkin;
        }
        windowRect = GUI.Window(0, windowRect, DoMyWindow, str);

    }

    void DoMyWindow(int windowID)
    {
        if (GUI.Button(new Rect(10, 30, 100, 60), "InitSDK"))
        {
#if UNITY_ANDROID
            AndroidChannelSDKMgr.InitSDK();
#endif
        }
        if (GUI.Button(new Rect(150, 30, 100, 60), "Login"))
        {
#if UNITY_ANDROID
            AndroidChannelSDKMgr.Login();
#endif
        }
        if (GUI.Button(new Rect(10, 110, 100, 60), "Logout"))
        {
#if UNITY_ANDROID
            AndroidChannelSDKMgr.Logout();
#endif
        }
        if (GUI.Button(new Rect(150, 110, 100, 60), "Exit"))
        {
#if UNITY_ANDROID
            AndroidChannelSDKMgr.Exit();
#endif
        }
        if (GUI.Button(new Rect(10, 190, 100, 60), "SetRoleData"))
        {
#if UNITY_ANDROID
            AndroidChannelSDKMgr.SetRoleData(ChannelUserInfo.Instance);
#endif
        }
        if (GUI.Button(new Rect(150, 190, 100, 60), "SendRoleData"))
        {
            SDKDefine.SendRoleDataType dataType = SDKDefine.SendRoleDataType.Create;
            int roleId = 0;
            string roleName = "";
            int roleLevel = 9;
            int zoneId = 4;
            string zoneName = "";
            ulong roleCTime = 90;
            ulong roleLevelMTime = 098676;
#if UNITY_ANDROID
            AndroidChannelSDKMgr.SendRoleData(dataType, roleId, roleName, roleLevel, zoneId, zoneName, roleCTime, roleLevelMTime);
#endif
        }

        if (GUI.Button(new Rect(10, 270, 240, 60), "Pay"))
        {
            PayInfo payInfo = null;
#if UNITY_ANDROID
            AndroidChannelSDKMgr.Pay(payInfo);
#endif
        }

        if (GUI.Button(new Rect(10, 350, 240, 60), "GetPayInfo"))
        {
#if UNITY_ANDROID
            AndroidChannelSDKMgr.GetPayInfo();
#endif
        }

        if (GUI.Button(new Rect(10, 430, 240, 60), "Charge"))
        {
            string itemName = "";
            int unitPrice = 89;
            int count = 89;
            string callBackInfo = "";
#if UNITY_ANDROID
            AndroidChannelSDKMgr.Charge(itemName, unitPrice, count, callBackInfo);
#endif
        }
    }

}
