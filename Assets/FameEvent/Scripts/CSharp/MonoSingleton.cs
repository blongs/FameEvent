
using UnityEngine;

namespace Assets.Scripts.Common
{
    /*
    /// <summary>
    ///     基类继承树中有MonoBehavrour类的单件实现，这种单件实现有利于减少对场景树的查询操作
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingleton<T> : MonoSingletonBase where T : Component
    {
        // 单件子类实例
        private static T _instance;

        // 在单件中，每个物件的destroyed标志设计上应该分割在不同的存储个空间中，因此，忽略R#的这个提示
        // ReSharper disable once StaticFieldInGenericType
        private static bool _destroyed;

        /// <summary>
        ///     获得单件实例，查询场景中是否有该种类型，如果有存储静态变量，如果没有，构建一个带有这个component的gameobject
        ///     这种单件实例的GameObject直接挂接在bootroot节点下，在场景中的生命周期和游戏生命周期相同，创建这个单件实例的模块
        ///     必须通过DestroyInstance自行管理单件的生命周期
        /// </summary>
        /// <returns>返回单件实例</returns>
        public static T GetInstance()
        {
            // 打包报错，暂时改成原来的
            ////wangying modify
            //if (Object.ReferenceEquals(_instance,null) && !_destroyed)
            if (_instance == null && !_destroyed)
            {
                try
                {
                    if (Boot.IsOnApplicationQuit)
                    {
                        return null;
                    }

                    GameObject go = null;
                    string typeName = typeof(T).Name;
                    if (Boot.HasInstance())
                    {
                        Transform child = Boot.GetInstance().transform.FindChild(typeName);
                        if (null != child)
                        {
                            go = child.gameObject;
                        }
                    }
                    else
                    {
                        go = GameObject.Find(typeName);
                    }

                    if (null == go)
                    {
                        go = new GameObject(typeName);
                        go.AddComponent<T>();
                        if (Boot.HasInstance())
                        {
                            go.transform.parent = Boot.GetInstance().transform;
                        }
                        ssLogger.Log("Create MonoSingleton instance: " + typeName);
                    }
                    else
                    {
                        _instance = go.GetComponent<T>();
                        if (null == _instance)
                        {
                            go.AddComponent<T>();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    MSDKLog.GetInstance().AddLog(MSDKLog.LogLevel.NORMAL, MSDKLog.LogType.OTHER, "MonoSingeleton.GetInstance, type=" + typeof(T).Name, ex);
                    Boot.GetInstance().ThrowRuntimeException("MonoSingeleton.GetInstance, type=" + typeof(T).Name + "  StackTrace:" + System.Environment.StackTrace);
                }
            }

            return _instance;
        }

        /// <summary>
        ///     删除单件实例,这种继承关系的单件生命周期应该由模块显示管理
        /// </summary>
        public static void DestroyInstance()
        {
            if (_instance != null)
            {
                Destroy(_instance.gameObject);
            }

            _destroyed = true;
            _instance = null;
        }

        /// <summary>
        ///     Awake消息，确保单件实例的唯一性
        /// </summary>
        protected override void Awake()
        {
            if (_instance != null && _instance.gameObject != gameObject)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
            }
            else if (_instance == null)
            {
                _instance = GetComponent<T>();
            }

            DontDestroyOnLoad(gameObject);
            base.Awake();

            Init();
        }

        /// <summary>
        ///     OnDestroy消息，确保单件的静态实例会随着GameObject销毁
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (_instance != null && _instance.gameObject == gameObject)
            {
                _instance = null;
            }
        }

        public override void DestroySelf()
        {
            if (!HasDisposed())
            {
                base.DestroySelf();
                _instance = null;
                Destroy(gameObject);
            }
        }

        public static bool HasInstance()
        {
            return _instance != null;
        }

        public virtual void Init()
        {
        }
    }

    public class MonoSingletonBase : MonoBehaviour
    {
        bool disposed = false;

        protected virtual void Awake() { disposed = false; }

        public virtual void DestroySelf() { disposed = true; }

        public virtual bool HasDisposed() { return disposed; }
    }
    */
}