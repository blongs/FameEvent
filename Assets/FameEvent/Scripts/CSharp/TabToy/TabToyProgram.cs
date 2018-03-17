using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using tabtoy;
using table;

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
        var dir = Directory.GetCurrentDirectory();
        using (var stream = new FileStream(Application.dataPath + "/StreamingAssets/TabToy/Config.bin", FileMode.Open))
        {
            stream.Position = 0;

            var reader = new tabtoy.DataReader(stream);

            if (!reader.ReadHeader())
            {
                Console.WriteLine("combine file crack!");
                return;
            }

            var config = new table.Config();
            table.Config.Deserialize(config, reader);

            // 直接通过下标获取或遍历
            var directFetch = config.Sample[2];

            // 添加日志输出或自定义输出
            config.TableLogger.AddTarget(new tabtoy.DebuggerTarget());

            // 取空时, 当默认值不为空时, 输出日志
            var nullFetchOutLog = config.GetSampleByID(0);

        }

    }
}
