using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using tabtoy;
using TableTest;


public class TableTestMsg : MsgBase
{
    public int sampleDefineId;
    public int blongsDefineId;
    public SampleDefine sampleDefine;
    public BlongsDefine blongsDefine;

    public TableTestMsg(int _sampleDefineId,SampleDefine _sampleDefine, int _blongsDefineId,BlongsDefine _blongsDefine)
    {
        sampleDefineId = _sampleDefineId;
        sampleDefine = _sampleDefine;
        blongsDefineId = _blongsDefineId;
        blongsDefine = _blongsDefine;
    }
}


public class TableTestLoader : TabToyBase
{

    private Config config;

    private void Awake()
    {
        msgIds = new ushort[] {
            (ushort) TabToyEvent.TableTestGetBlongs,
            (ushort) TabToyEvent.TableTestGetSample,
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
        using (var stream = new FileStream(Application.dataPath + "/StreamingAssets/DataBin/TableTest.bin", FileMode.Open))
        {
            stream.Position = 0;

            var reader = new tabtoy.DataReader(stream);

            if (!reader.ReadHeader())
            {
                Console.WriteLine("combine file crack!");
                return;
            }

            config = new TableTest.Config();
            TableTest.Config.Deserialize(config, reader);
        }

    }

    private BlongsDefine GetBlongsDefineById(int _id)
    {
        return config.GetBlongsByNEWID(_id);
    }

    private SampleDefine GetSampleDefineById(int _id)
    {
        return config.GetSampleByID(_id);
    }


    public override void ProcessEvent(MsgBase tmpMsg)
    {
        switch (tmpMsg.msgId)
        {
            case (ushort)TabToyEvent.TableTestGetSample:
                {
                    TableTestMsg tmp = (TableTestMsg)tmpMsg;

                    SampleDefine tmpsampleDefine = GetSampleDefineById(tmp.sampleDefineId);

                    tmp.msgId = (ushort)TabToyEvent.TableTestBackSample;
                    tmp.sampleDefine = tmpsampleDefine;

                    SendMsg(tmp);


                }
                break;

            case (ushort)TabToyEvent.TableTestGetBlongs:
                {
                    TableTestMsg tmp = (TableTestMsg)tmpMsg;

                    BlongsDefine tmpsampleDefine = GetBlongsDefineById(tmp.blongsDefineId);

                    tmp.msgId = (ushort)TabToyEvent.TableTestBackBlongs;
                    tmp.blongsDefine = tmpsampleDefine;
                    SendMsg(tmp);
                }
                break;


        }
    }
}
