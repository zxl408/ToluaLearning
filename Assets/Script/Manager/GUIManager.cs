using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zxl.Res;
public class GUIManager : MonoBehaviour
{
    public Dictionary<string, UIbase> UIs = new Dictionary<string, UIbase>();
    static GUIManager ins;
    private void Awake()
    {
        ins = this;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadUI(string uiName, System.Action<UIbase> onfinsh = null)
    {
        var relativeAssetName = MainGameConfig.GetRelativeUIAssetName(uiName);
        ResManager.GetResManager().AssetBundleManager.LoadAssetBundle(MainGameConfig.GetRelativeUIAssetName(uiName), (assetBundleItem) =>
       {
           var assetName = "assets/" + relativeAssetName.Replace(".unity3d", ".prefab");
           assetBundleItem.Register();
           foreach (var it in assetBundleItem.Assetbundle.GetAllAssetNames())
               print(it);
           print(assetName);
           var ui = Instantiate(assetBundleItem.Assetbundle.LoadAsset<GameObject>(assetName));
           var uiComponent = ui.AddComponent(System.Type.GetType(uiName)) as UIbase;
           uiComponent.assetBundleItem = assetBundleItem;
           UIs.Add(uiName, uiComponent);
           if (onfinsh != null)
               onfinsh(uiComponent);
       });
    }
    public void LoadUI<T>(string uiName, System.Action<T> onfinsh = null) where T : UIbase
    {
        ResManager.GetResManager().AssetBundleManager.LoadAssetBundle(MainGameConfig.UIUrl, (assetBundleItem) =>
        {
            assetBundleItem.Register();
            var ui = Instantiate(assetBundleItem.Assetbundle.mainAsset) as GameObject;
            var uiComponent = ui.AddComponent<T>();
            uiComponent.assetBundleItem = assetBundleItem;
            UIs.Add(uiName, uiComponent);
            if (onfinsh != null)
                onfinsh(uiComponent);
        });
    }
    public void CloesUI(string uiName)
    {
        if (UIs.ContainsKey(uiName))
        {
            UIs[uiName].assetBundleItem.Delete();
            Destroy(UIs[uiName]);
        }
    }

    public static GUIManager GetIns()
    {
        if (ins == null)
        {
            var go = new GameObject("GUIManager");
            go.AddComponent<GUIManager>();
        }
        return ins;
    }
}
