using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class AssetBundleItem
{
    public AssetBundle assetbundle {
         get;private set;
    }
    
    public int referenceCount;
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
            assetbundle.Unload(false);
    }
    public AssetBundleItem(AssetBundle assetbundle) {
        this.assetbundle = assetbundle;
    }
}
public class AssetBundleManager : MonoBehaviour
{
    private Dictionary<string, AssetBundleItem> assetBundles = new Dictionary<string, AssetBundleItem>();
    public AssetBundleManifest manifest;
    public string ManifestUri;
    public string assetBundleRootUri;
    public static AssetBundleManager Ins;
    public bool isInit = false;
    private void Awake()
    {
        Ins = GetComponent<AssetBundleManager>();
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {

    }
    void Init()
    {
        StartCoroutine(LoadManifest());
    }
    IEnumerator LoadManifest()
    {
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(ManifestUri);
        yield return request.Send();
        var assetbundle = DownloadHandlerAssetBundle.GetContent(request);

        if (assetbundle != null)
        {
            manifest = assetbundle.LoadAsset<AssetBundleManifest>("Win");
            isInit = true;
            
        }
        else {
            isInit = false;
        }



    }
    public void LoadAssetBundle(string assetbundleName)
    {
        var array= manifest.GetAllDependencies(assetbundleName);
        for (int i = 0; i < array.Length; i++) {
            AssetBundleItem item;
            if (assetBundles.TryGetValue(array[i], out item))
            {
                if (item != null && item.referenceCount > 0) {
                    continue;
                }
            }
            else {

            }
        }
    }
    IEnumerator LoadAsset(string assetName) {
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(Path.Combine(assetBundleRootUri,assetName));
        yield return request.Send();
        var bundle = DownloadHandlerAssetBundle.GetContent(request);
        if (!assetBundles.ContainsKey(assetName))
        {
            assetBundles[assetName] = new AssetBundleItem(bundle);
        }
        else {
            assetBundles.Add(assetName, new AssetBundleItem(bundle));
        }      
       
    }
   
}
