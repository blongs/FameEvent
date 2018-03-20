using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using tabtoy;
using TableTest;

public class TabToyProgram : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Test();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void Test()
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

            var config = new TableTest.Config();
            TableTest.Config.Deserialize(config, reader);

            // 直接通过下标获取或遍历
            var directFetch = config.Sample[2];

            // 添加日志输出或自定义输出
            config.TableLogger.AddTarget(new tabtoy.DebuggerTarget());

            // 取空时, 当默认值不为空时, 输出日志
            SampleDefine sampleDefine = config.GetSampleByID(101);
            Debug.Log("sampleDefine =" + sampleDefine.Name);

        }

    }
}
