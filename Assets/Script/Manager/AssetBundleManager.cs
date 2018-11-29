using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class AssetBundleItem
{
    private AssetBundle assetbundle;
    public AssetBundle Assetbundle
    {
        get
        {
            return assetbundle;
        }
        private set
        {
            assetbundle = value;
        }
    }
    public bool IsVailed
    {
        get
        {
            return Assetbundle != null;
        }
    }
    public int referenceCount;
    public List<string> depends = new List<string>();
    public void Register()
    {
        referenceCount++;
    }
    public void Delete()
    {
        referenceCount--;
        CheckReference();
    }
    public void CheckReference()
    {
        if (referenceCount <= 0)
        {
            assetbundle.Unload(false);
            Assetbundle = null;
        }
    }

    public AssetBundleItem(AssetBundle assetbundle)
    {
        this.assetbundle = assetbundle;
    }
}
public class AssetBundleManager : MonoBehaviour
{
    private Dictionary<string, AssetBundleItem> assetBundles = new Dictionary<string, AssetBundleItem>();
    public AssetBundleManifest manifest;
    public string ManifestUri;
    public string assetBundleRootUri;
    public static AssetBundleManager ins;
    public static AssetBundleManager Ins
    {
        get
        {
            if (ins == null)
            {
                var go = new GameObject("AssetBundleManager");
                ins = go.AddComponent<AssetBundleManager>();
            }
            return ins;
        }
    }
    public bool isInit = false;
    private void Awake()
    {
        ins = GetComponent<AssetBundleManager>();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {

    }
    public void Init(System.Action<bool> onfinsh)
    {
        assetBundleRootUri = MainGameConfig.RemoteResUri;
        ManifestUri = new System.Uri(Path.Combine(assetBundleRootUri, MainGameConfig.osDir)).AbsoluteUri;
        StartCoroutine(LoadManifest(onfinsh));
    }
    IEnumerator LoadManifest(System.Action<bool> onfinsh)
    {
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(ManifestUri);
        yield return request.Send();
        var assetbundle = DownloadHandlerAssetBundle.GetContent(request);

        if (assetbundle != null)
        {
            manifest = assetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            isInit = true;       
        }
        else
        {
            isInit = false;      
        }
        if (onfinsh != null)
            onfinsh(isInit);
    }
    public void LoadLuaFile(string assetbundleName, System.Action<AssetBundle> onfinsh)
    {
        LoadAssetBundle(assetbundleName, (it) =>
        {
            AssetBundle bundle = null;
            if (it != null)
            {
                it.Register();//注册
                bundle = it.Assetbundle;
            }
            if (onfinsh != null)
            {
                onfinsh(bundle);
            }
        });
    }
    public void LoadAssetBundle(string assetbundleName, System.Action<AssetBundleItem> onfinsh)
    {
        var array = manifest.GetAllDependencies(assetbundleName);
        AssetBundleItem Item;
        if (assetBundles.TryGetValue(assetbundleName, out Item))
        {
            if (Item != null && Item.IsVailed && IsLoadedAllDepends(assetbundleName))//是否有
            {
                if (onfinsh != null)
                {
                    onfinsh(Item);
                }
                return;
            }
        }
        if (array.Length == 0)
        {
            StartCoroutine(LoadAsset(assetbundleName, (name, isSuccess) =>
            {
                if (isSuccess)
                {
                    if (onfinsh != null)
                    {
                        onfinsh(assetBundles[name]);
                    }
                }
                else
                {
                    if (onfinsh != null)
                    {
                        onfinsh(null);
                    }
                }
            }));
        }
        int count = array.Length;
        for (int i = 0; i < array.Length; i++)//开始加载依赖项
        {
            AssetBundleItem dependItem;
            if (assetBundles.TryGetValue(array[i], out dependItem))
            {
                if (dependItem != null && dependItem.referenceCount > 0)
                {
                    continue;
                }
            }
            else
            {
                StartCoroutine(LoadAsset(array[i], (name, isSuccess) =>
                {
                    if (isSuccess)
                    {
                        count--;
                        if (count == 0)
                        {
                            StartCoroutine(LoadAsset(assetbundleName, (name1, isSuccess1) =>
                            {
                                if (isSuccess1)
                                {
                                    if (onfinsh != null)
                                    {
                                        onfinsh(assetBundles[name1]);
                                    }
                                }
                                else
                                {
                                    if (onfinsh != null)
                                    {
                                        onfinsh(null);
                                    }
                                }
                            }));
                        }
                    }
                }));
            }
        }
    }

    IEnumerator LoadAsset(string assetName, System.Action<string, bool> onfinsh)
    {
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(Path.Combine(assetBundleRootUri, assetName));
        var async = request.Send();
        yield return async;
        if (async.isDone)
        {
            var bundle = DownloadHandlerAssetBundle.GetContent(request);
            if (!assetBundles.ContainsKey(assetName))
            {
                assetBundles[assetName] = new AssetBundleItem(bundle);
            }
            else
            {
                assetBundles.Add(assetName, new AssetBundleItem(bundle));
            }
            if (onfinsh != null)
            {
                onfinsh(assetName, true);
            }
        }
        else
        {
            Debug.LogError(assetName + " :Load error");
            if (onfinsh != null)
            {
                onfinsh(assetName, false);
            }
        }


    }

    bool IsLoadedAllDepends(string assetName)
    {
        var array = manifest.GetAllDependencies(assetName);
        AssetBundleItem item;
        foreach (var it in array)
        {
            if (assetBundles.TryGetValue(it, out item) && item.Assetbundle != null)
            {
            }
            else
            {
                return false;
            }
        }
        return true;
    }
}
