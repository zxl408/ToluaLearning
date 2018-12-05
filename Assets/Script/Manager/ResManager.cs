using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
namespace Zxl.Res
{

    /// <summary>
    /// 资源管理器
    /// </summary>
    public class ResManager : MonoBehaviour
    {
        private static ResManager resManager;
        public AssetBundleManager AssetBundleManager;
        public LuaManager luaManager;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            resManager = this;
        }

        public void InitRes(System.Action<bool> onfinsh)
        {
            InitLocalCach();
            AssetBundleManager = AssetBundleManager.Ins;

            AssetBundleManager.Init((isSucess) =>
            {
                if (isSucess)
                {
                    DownLoadLua(onfinsh);
                }
                else {
                    Debug.LogError("下载资源失败!!!");
                }
            });

        }
        void InitLocalCach()
        {
            if (!System.IO.Directory.Exists(MainGameConfig.LocalResPath))
            {
                System.IO.Directory.CreateDirectory(MainGameConfig.LocalResPath.Replace("file:///", ""));
            }
            if (!System.IO.Directory.Exists(MainGameConfig.LocalLuaPath))
            {
                System.IO.Directory.CreateDirectory(MainGameConfig.LocalLuaPath.Replace("file:///", ""));
            }
        }
        void DownLoadLua(System.Action<bool> onfinsh)
        {
            var array = AssetBundleManager.manifest.GetAllAssetBundles();
            var list = array.ToList().FindAll((it) => it.Substring(0, 3) == "lua");
            int count = list.Count;
            foreach (var assetName in list)
            {
                AssetBundleManager.LoadLuaFile(assetName, (bundle =>
                {
                    if (bundle != null)
                    {                        
                        string fielName = Path.GetFileNameWithoutExtension(assetName);
                        print("name: " + fielName);
                        LuaFileUtils.Instance.AddSearchBundle(fielName, bundle);
                        count--;
                        if (count == 0)
                        {
                            if (onfinsh != null)
                                onfinsh(true);
                        }
                    }
                    else
                    {
                        if (onfinsh != null)
                            onfinsh(false);
                    }
                }));
            }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public static ResManager GetResManager()
        {
            if (resManager == null)
            {
                resManager = GameObject.FindObjectOfType<ResManager>();
                if (resManager == null)
                    resManager = (new GameObject("ResManager")).AddComponent<ResManager>();

            }
            return resManager;
        }
    }

}