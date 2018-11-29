using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
        resManager = this;
    }
     
   public  void InitRes(System.Action<bool> onfinsh)
    {
        AssetBundleManager = AssetBundleManager.Ins;
        AssetBundleManager.Init((isSucess)=> {
            DownLoadLua(onfinsh);
        });
        
    }
    void DownLoadLua(System.Action<bool> onfinsh)
    {
        var array = AssetBundleManager.manifest.GetAllAssetBundles();
        var list = array.ToList().FindAll((it) => it.Substring(0, 3) == "lua");
        int count = list.Count;
        foreach (var assetName in list)
        {
            AssetBundleManager.LoadLuaFile(assetName, (bundle => {
                if (bundle != null)
                {
                    print("AddSearchBundle "+assetName);
                    LuaFileUtils.Instance.AddSearchBundle(assetName, bundle);
                    count--;
                    if (count == 0)
                    {
                        if (onfinsh != null)
                            onfinsh(true);
                    }
                }
                else {
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
