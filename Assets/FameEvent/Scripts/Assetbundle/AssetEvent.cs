using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AssetEvent
{
    HankResource = ManagerId.AssetManager + 1,
    BackTest,
    ReleaseSingleObj,
    ReleaseBundleObj,
    ReleaseScenceObj,
    ReleaseSingleBundle,
    ReleaseScenceBundle,
    ReleaseAll,


}
public class HankAssetResource : MsgBase
{

    public string scenceName;

    public string bundleName;

    public string resourceName;

    public ushort backMsgId;

    public bool isSingle;

    public HankAssetResource(bool tmpSingle,ushort msgId,string  tmpSenceName,string tmpBundle,string tmpRes, ushort tmpBackId)
    {
        this.isSingle = tmpSingle;
        this.msgId = msgId;
        this.scenceName = tmpSenceName;
        this.bundleName = tmpBundle;
        this.resourceName = tmpRes;
        this.backMsgId = tmpBackId;

        
    }

   

}

public class HankAssetResourceBack:MsgBase
{
    public Object[] value;

    public HankAssetResourceBack()
    {
        this.msgId = 0;
        this.value = null;
    }

    public void Changer(ushort msgId,params Object[] tmpValue)
    {
        this.msgId = msgId;
        this.value = tmpValue;
    }

    public void Changer(ushort msgId)
    {
        this.msgId = msgId;
    }

    public void Changer(params Object[] tmpValue)
    {
        this.value = tmpValue;
    }
}
