using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using tabtoy;
using TableCharactor;


public class TableCharactorMsg : MsgBase
{
    public int id;
    public CharactorDefine charactorDefine;

    public TableCharactorMsg(int _id, CharactorDefine _charactorDefine)
    {
        id = _id;
        charactorDefine = _charactorDefine;
    }
}


public class TableCharactorLoader : TabToyBase
{

    private Config config;

    private void Awake()
    {
        msgIds = new ushort[] {
            (ushort) TabToyEvent.TableCharactorGet,
        };
        RegistSelf(this, msgIds);
    }
    // Use this for initialization
    void Start()
    {
        LoadBinData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadBinData()
    {
        var dir = Directory.GetCurrentDirectory();
        using (var stream = new FileStream(Application.dataPath + "/StreamingAssets/DataBin/Charactor.bin", FileMode.Open))
        {
            stream.Position = 0;

            var reader = new tabtoy.DataReader(stream);

            if (!reader.ReadHeader())
            {
                Console.WriteLine("combine file crack!");
                return;
            }

            config = new TableCharactor.Config();
            TableCharactor.Config.Deserialize(config, reader);
        }

    }

    private  CharactorDefine GetCharactorDefine(int _id)
    {
        return config.GetCharactorByID(_id);
    }



    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)TabToyEvent.TableCharactorGet:
                {
                    TableCharactorMsg tmp = (TableCharactorMsg)tmpMsg;
                    CharactorDefine tmpsampleDefine = GetCharactorDefine(tmp.id);
                    tmp.msgId = (ushort)TabToyEvent.TableCharactorBack;
                    tmp.charactorDefine = tmpsampleDefine;
                    SendMsg(tmp);


                }
                break;
            default:
                break;
        }
    }
}
