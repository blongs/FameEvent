using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using MiNiJSON;
public class ChannelUserInfo
{
    public string AccountId;  //账号
    public string ChannelId;  //渠道id
    public string ChannelUserId;//渠道得用户id
    public string RoleId;//角色id
    public string RoleName;//角色名字
    public string ZoneId;//区id
    public string ZoneName;//区名字
    public int RoleLevel; // 角色等级
    private static ChannelUserInfo _instance;
    public static ChannelUserInfo Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ChannelUserInfo();
            }
            return _instance;
        }
    }

    public SDKOnlineUser SDKUser = new SDKOnlineUser();

    public string GetReYunFirstInInfo()
    {
        Dictionary<string, string> reyunDict = new Dictionary<string, string>();
        reyunDict["accountId"] = ChannelUserInfo.Instance.RoleId;
        reyunDict["accountType"] = ChannelUserInfo.Instance.SDKUser.getChannelId();
        reyunDict["gender"] = "";
        reyunDict["age"] = ChannelUserInfo.Instance.RoleLevel.ToString();
        reyunDict["serverId"] = ChannelUserInfo.Instance.ZoneId;
        reyunDict["roleName"] = ChannelUserInfo.Instance.RoleName;
        string strReYunAccountInfo = Json.Serialize(reyunDict);
        return strReYunAccountInfo;
    }

    public string GetReYunLoginInfo()
    {
        Dictionary<string, string> reyunDict = new Dictionary<string, string>();
        reyunDict["accountId"] = ChannelUserInfo.Instance.RoleId;
        reyunDict["gender"] = "";
        reyunDict["age"] = ChannelUserInfo.Instance.RoleLevel.ToString();
        reyunDict["serverId"] = ChannelUserInfo.Instance.ZoneId;
        reyunDict["roleName"] = ChannelUserInfo.Instance.RoleName;
        reyunDict["level"] = ChannelUserInfo.Instance.RoleLevel.ToString();
        string strReYunLoginInfo = Json.Serialize(reyunDict);
        return strReYunLoginInfo;
    }
}



