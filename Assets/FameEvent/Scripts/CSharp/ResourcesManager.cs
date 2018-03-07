using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/*
什么时候才是UnusedAssets?
看一个例子：
Object obj = Resources.Load("MyPrefab");
GameObject instance = Instantiate(obj) as GameObject;
.........
Destroy(instance);
创建随后销毁了一个Prefab实例，这时候 MyPrefab已经没有被实际的物体引用了，但如果这时：
Resources.UnloadUnusedAssets();
内存并没有被释放，原因：MyPrefab还被这个变量obj所引用
这时候：
obj  = null;
Resources.UnloadUnusedAssets();
这样才能真正释放Assets对象
所以：UnusedAssets不但要没有被实际物体引用，也要没有被生命周期内的变量所引用，才可以理解为 Unused(引用计数为0)
所以所以：如果你用个全局变量保存你Load的Assets，又没有显式的设为null，那 在这个变量失效前你无论如何UnloadUnusedAssets也释放不了那些Assets的。
如果你这些Assets又不是从磁盘加载的，那除了 UnloadUnusedAssets或者加载新场景以外没有其他方式可以卸载之。
*/
public class ResourcesManager : MonoBehaviour
{


    static ResourcesManager instance;

    public static ResourcesManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ResourcesManager");
                if (MsgCenter.Instance != null)
                {
                    instance = go.AddComponent<ResourcesManager>();
                    go.transform.parent = MsgCenter.Instance.transform;
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 场景，类型，具体的物体
    /// </summary>
    public Dictionary<string, Dictionary<string, Dictionary<string, Object>>> allScenceResources = new Dictionary<string, Dictionary<string, Dictionary<string, Object>>>();

    public Object GetSceneResources(string _sceneName, string _bundleName, string _res)
    {
        //这个场景已经有了
        if (allScenceResources.ContainsKey(_sceneName))
        {
            Dictionary<string, Dictionary<string, Object>> sceneceResources = allScenceResources[_sceneName];
            //这个类型也已经有了
            if (sceneceResources.ContainsKey(_bundleName))
            {
                Dictionary<string, Object> typeResources = sceneceResources[_bundleName];
                //已经有了这个资源
                if (typeResources.ContainsKey(_res))
                {
                    return typeResources[_res];
                }
                else
                {
                    Object ob = LoadSigleScenesObj(_sceneName, _bundleName,_res);
                    typeResources.Add(_res, ob);
                    return ob;
                }
            }
            //这个类型还没有
            else
            {
                Object ob = LoadSigleScenesObj(_sceneName, _bundleName, _res);
                Dictionary<string, Object> typeResources = new Dictionary<string, Object>();
                typeResources.Add(_res, ob);
                sceneceResources.Add(_bundleName, typeResources);
                return ob;
            }
        }
        //这个场景还没有
        else
        {
            Object ob = LoadSigleScenesObj(_sceneName, _bundleName, _res);
            Dictionary<string, Object> resources = new Dictionary<string, Object>();
            resources.Add(_res, ob);
            Dictionary<string, Dictionary<string, Object>> typeResources = new Dictionary<string, Dictionary<string, Object>>();
            typeResources.Add(_bundleName, resources);
            allScenceResources.Add(_sceneName, typeResources);
            return ob;
        }
    }

    public void ReleaseAllSceneObject(string _sceneName)
    {
        if (allScenceResources.ContainsKey(_sceneName))
        {
            allScenceResources[_sceneName] = null;
        }
        else
        {
            Debug.LogError("内存池中没有" + _sceneName + "场景");
        }
        Resources.UnloadUnusedAssets();
    }


    public void ReleaseSceneTypeObject(string _sceneName, string _bundleName)
    {
        //这个场景已经有了
        if (allScenceResources.ContainsKey(_sceneName))
        {
            Dictionary<string, Dictionary<string, Object>> sceneceResources = allScenceResources[_sceneName];
            //这个类型也已经有了
            if (sceneceResources.ContainsKey(_bundleName))
            {
               sceneceResources[_bundleName] = null;
            }
            //这个类型还没有
            else
            {
                Debug.LogError("内存池中没有" + _sceneName + "场景," + _bundleName + "类型");
            }
        }
        //这个场景还没有
        else
        {
            Debug.LogError("内存池中没有" + _sceneName + "场景");
        }
        Resources.UnloadUnusedAssets();
    }


    public void ReleaseSingleSceneObject(string _sceneName,string _bundleName, string _res)
    {
        //这个场景已经有了
        if (allScenceResources.ContainsKey(_sceneName))
        {
            Dictionary<string, Dictionary<string, Object>> sceneceResources = allScenceResources[_sceneName];
            //这个类型也已经有了
            if (sceneceResources.ContainsKey(_bundleName))
            {
                Dictionary<string, Object> typeResources = sceneceResources[_bundleName];
                //已经有了这个资源
                if (typeResources.ContainsKey(_res))
                {
                    typeResources[_res] = null;
                    typeResources.Remove(_res);
                }
                else
                {
                    Debug.LogError("内存池中没有" + _sceneName + "场景,"+ _bundleName+ "类型，"+_res+"的资源");
                }
            }
            //这个类型还没有
            else
            {
                Debug.LogError("内存池中没有" + _sceneName + "场景," + _bundleName + "类型");
            }
        }
        //这个场景还没有
        else
        {
            Debug.LogError("内存池中没有" + _sceneName + "场景");
        }
    }


    private Object LoadSigleScenesObj(string _sceneName, string _bundleName, string _res)
    {
        string path = FrameTools.ResoucesParentPath + Path.DirectorySeparatorChar + _sceneName + Path.DirectorySeparatorChar + _bundleName + Path.DirectorySeparatorChar + _res;
        Object ob = Resources.Load(path);
        return ob;
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
