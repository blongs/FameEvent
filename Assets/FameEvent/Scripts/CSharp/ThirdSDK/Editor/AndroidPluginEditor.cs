using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;

public class AndroidPluginEditor
{
    private static string[] tokens = new string[]
        {
            "<!-- ACTIVITIES -->",
            "<!-- META-DATA -->",
            "<!-- PERMISSIONS -->",
            "<!-- FIND-REPLACE -->"
        };

    private static Dictionary<string, int> addonContentDict = new Dictionary<string, int>()
    {
        {"<!-- META-DATA -->",0},
        {"<!-- ACTIVITIES -->",0},
        {"<!-- PERMISSIONS -->",1}
    };

    /// <summary>
    /// android sdk 根目录
    /// </summary>
    private static string myAndroidSdkPluginsPath = Application.dataPath + "/Plugins/AndroidSDKPlugins";
    /// <summary>
    /// unity android根目录
    /// </summary>
    private static string unityAndroidPluginsPath = Application.dataPath + "/Plugins/Android";

    /// <summary>
    /// 生成sdk插件
    /// </summary>
    public static void GenerateSDKPlugin(string title, string baseManifest, List<string> pluginList)
    {
        // 先拷贝
        CopyAndMergeFile(pluginList, true);
        GenerateManifest(baseManifest, pluginList);
        AssetDatabase.CreateFolder("Assets/Plugins/Android", title);
    }

    /// <summary>
    /// 生成manifest
    /// </summary>
    /// <returns></returns>
    public static bool GenerateManifest(string baseManifest, List<string> pluginList)
    {
        string text = GetBaseManifestFile(baseManifest);
        if (text == null)
        {
            return false;
        }
        List<string> allManifestFiles = GetManifestFiles(pluginList);
        Dictionary<string, List<string>> manifestContent = new Dictionary<string, List<string>>();
        foreach (string current in allManifestFiles)
        {
            Dictionary<string, List<string>> addon = ExtractAttributesFromManifest(current);
            MergeAttributeDictionaries(manifestContent, addon);
        }
        if (manifestContent.Count == 0)
        {
            return false;
        }
        foreach (string manifestToken in manifestContent.Keys)
        {
            if (manifestToken != null)
            {
                int num;
                if (addonContentDict.TryGetValue(manifestToken, out num))
                {
                    if (num != 0)
                    {
                        if (num == 1)
                        {
                            List<string> mainfestValueList = new List<string>(manifestContent[manifestToken]);
                            mainfestValueList.Insert(0, "</application>");
                            mainfestValueList.Insert(1, string.Empty);
                            mainfestValueList.Add(string.Empty);
                            text = text.Replace("</application>", string.Join("\n", mainfestValueList.ToArray()));
                        }
                    }
                    else
                    {
                        List<string> mainfestValueList = new List<string>(manifestContent[manifestToken]);
                        mainfestValueList.Insert(0, string.Empty);
                        mainfestValueList.Add("</application>");
                        text = text.Replace("</application>", string.Join("\n", mainfestValueList.ToArray()));
                    }
                }
            }
        }
        if (!manifestContent.ContainsKey("<!-- FIND-REPLACE -->"))
        {
            manifestContent["<!-- FIND-REPLACE -->"] = new List<string>();
        }
        //manifestContent["<!-- FIND-REPLACE -->"].Add("<fr><find><![CDATA[ForwardNativeEventsToDalvik\" android:value=\"false\" />]]></find><replace><![CDATA[ForwardNativeEventsToDalvik\" android:value=\"true\" />]]></replace></fr>");
        if (manifestContent.ContainsKey("<!-- FIND-REPLACE -->"))
        {
            HandleFindAndReplacers(ref text, manifestContent["<!-- FIND-REPLACE -->"]);
        }
        string path = unityAndroidPluginsPath + "/AndroidManifest.xml";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        text = text.Replace("SDKRegionPackageName", PlayerSettings.applicationIdentifier);
        File.WriteAllText(path, text);

        AssetDatabase.Refresh();
        return true;
    }

    /// <summary>
    /// 拷贝融合资源
    /// </summary>
    /// <param name="pluginList"></param>
    /// <param name="delete"></param>
    public static void CopyAndMergeFile(List<string> pluginList, bool delete)
    {
        int assetPathLength = Application.dataPath.Length - 6;      // 7: Assets
        if (delete && Directory.Exists(unityAndroidPluginsPath))
            AssetDatabase.DeleteAsset(unityAndroidPluginsPath.Substring(assetPathLength));

        foreach (var pluginDirectoryName in pluginList)
        {
            string pluginPath = myAndroidSdkPluginsPath + "/" + pluginDirectoryName;
            int pluginPathLength = pluginPath.Length;
            string[] pluginFiles = Directory.GetFiles(pluginPath, "*.*", SearchOption.AllDirectories);
            foreach (var v in pluginFiles)
            {
                string pluginFile = v.Replace("\\", "/");
                string fileExt = Path.GetExtension(pluginFile).ToLower();
                string fileName = Path.GetFileName(pluginFile).ToLower();
                if (fileExt != ".meta" && fileName != "manifest.xml" && fileName != "manifest_replace.xml")
                {
                    string newPluginFile = unityAndroidPluginsPath + pluginFile.Substring(pluginPathLength);
                    if (File.Exists(newPluginFile))
                    {
                        Debug.LogWarning("拷贝sdk资源到Android目录时有重复" + pluginFile);
                    }
                    else
                    {
                        // copy
                        string parentDir = Path.GetDirectoryName(newPluginFile);
                        if (!Directory.Exists(parentDir))
                            Directory.CreateDirectory(parentDir);
                        pluginFile = pluginFile.Substring(assetPathLength);
                        newPluginFile = newPluginFile.Substring(assetPathLength);
                        Debug.Log(string.Format("拷贝资源从{0}到{1}", pluginFile, newPluginFile));
                        AssetDatabase.CopyAsset(pluginFile, newPluginFile);
                    }
                }
            }
        }
        AssetDatabase.Refresh();
    }

    /// <summary>
    /// 获取base manifest
    /// </summary>
    /// <param name="baseManifest"></param>
    /// <returns></returns>
    private static string GetBaseManifestFile(string baseManifest)
    {
        string path = myAndroidSdkPluginsPath + "/" + baseManifest + ".xml";
        if (!File.Exists(path))
        {
            throw new System.Exception("找不到" + baseManifest + ".xml");
        }
        return File.ReadAllText(path);
    }

    private static List<string> GetManifestFiles(List<string> pluginList)
    {
        List<string> results = new List<string>();
        foreach (var v in pluginList)
        {
            string manifestPath = "";
            manifestPath = myAndroidSdkPluginsPath + "/" + v + "/Manifest.xml";
            bool exist = File.Exists(manifestPath);
            if (exist)
                results.Add(manifestPath);
        }
        return results;
    }

    private static Dictionary<string, List<string>> ExtractAttributesFromManifest(string path)
    {
        Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
        string key = string.Empty;
        bool flag = false;
        string[] addonContent = File.ReadAllLines(path);
        for (int i = 0; i < addonContent.Length; i++)
        {
            if (addonContent[i].Length != 0)
            {
                for (int j = 0; j < tokens.Length; j++)
                {
                    if (addonContent[i].Contains(tokens[j]))
                    {
                        key = tokens[j];
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    flag = false;
                }
                else
                {
                    if (!dictionary.ContainsKey(key))
                    {
                        dictionary.Add(key, new List<string>());
                    }
                    string content = addonContent[i];//.Replace(kPackageNameKey, PlayerSettings.bundleIdentifier);
                    content.Trim();
                    if (content.EndsWith(" />"))
                    {
                        content = content.Replace(" />", "/>");
                    }
                    dictionary[key].Add("\t" + content);
                }
            }
        }
        return dictionary;
    }

    private static void MergeAttributeDictionaries(Dictionary<string, List<string>> root, Dictionary<string, List<string>> addon)
    {
        foreach (KeyValuePair<string, List<string>> addonItem in addon)
        {
            if (!root.ContainsKey(addonItem.Key))
            {
                root.Add(addonItem.Key, addonItem.Value);
            }
            else
            {
                foreach (string addonItemValue in addonItem.Value)
                {
                    if (!(addonItem.Key == "<!-- PERMISSIONS -->") || !root[addonItem.Key].Contains(addonItemValue))
                    {
                        root[addonItem.Key].Add(addonItemValue);
                    }
                }
            }
        }
    }

    private static void HandleFindAndReplacers(ref string allTextOfManifest, List<string> replacers)
    {
        foreach (string current in replacers)
        {
            Dictionary<string, string> dictionary = ParseFindAndReplacer(current.Trim());
            string text = dictionary["find"];
            string newValue = dictionary["replace"].Replace("$1", text);
            allTextOfManifest = allTextOfManifest.Replace(text, newValue);
        }
    }

    private static Dictionary<string, string> ParseFindAndReplacer(string replacer)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string key = string.Empty;
        StringReader input = new StringReader(replacer.Trim());
        XmlReader xmlReader = XmlReader.Create(input);
        while (xmlReader.Read())
        {
            switch (xmlReader.NodeType)
            {
                case XmlNodeType.Element:
                    key = xmlReader.Name;
                    break;
                case XmlNodeType.CDATA:
                    dictionary.Add(key, xmlReader.Value);
                    break;
            }
        }
        return dictionary;
    }
}
