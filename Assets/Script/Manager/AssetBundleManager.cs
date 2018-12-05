using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;
namespace Zxl.Res
{
    /// <summary>
    /// 资源对比
    /// </summary>
    public class ResourceContrast
    {
        /// <summary>
        /// 比对信息
        /// </summary>
        public class ContrastInfo
        {
            public string assetName;
            public string assetMd5;
            public string assetSize;
            private string content;
            public override string ToString()
            {
                if (string.IsNullOrEmpty(content))
                    content = string.Format("{0}\t{1}\t{2}\r\n", assetName, assetMd5, assetSize);
                return content;
            }
            public ContrastInfo()
            {
            }
            public ContrastInfo(string content)
            {
                var item = content.Split('\t');
                if (item.Length >= 3)
                {
                    assetName = item[0];
                    assetMd5 = item[1];
                    assetSize = item[2];
                    Debug.Log(string.Format("{0} {1} {2}", assetName, assetMd5, assetSize));
                }
            }
        }
        Dictionary<string, ContrastInfo> ContrastInfos = new Dictionary<string, ContrastInfo>();
        public ResourceContrast() { }
        public ResourceContrast(string text)
        {
            var array = text.Split('\n');
            for (int i = 0; i < array.Length; i++)
            {
                ContrastInfo info = new ContrastInfo(array[i]);
                if (!string.IsNullOrEmpty(info.assetName))
                    ContrastInfos[info.assetName] = info;
            }
        }
        public ContrastInfo GetContrastInfo(string assetName)
        {
            ContrastInfo info = null;
            if (!ContrastInfos.TryGetValue(assetName, out info))
            {
                Debug.LogError("Not find " + assetName);
            }
            return info;
        }
    }
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
        public ResourceContrast remoteResourceContrast;
        public ResourceContrast localResourceContrast = new ResourceContrast();
        private Dictionary<string, AssetBundleItem> assetBundles = new Dictionary<string, AssetBundleItem>();
        private Dictionary<string, string[]> dependAssets = new Dictionary<string, string[]>();
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
            WWW localResource = new WWW(MainGameConfig.LocalLuaPath);
            Debug.LogError(MainGameConfig.LocalLuaPath);
            yield return localResource;
            if (string.IsNullOrEmpty(localResource.error))
            {
                if (localResource.isDone)
                {
                    localResourceContrast = new ResourceContrast(localResource.text);
                }
            }
            WWW remoteResource = new WWW(MainGameConfig.RemoteResourcelistUrl);
            Debug.LogError(MainGameConfig.RemoteResourcelistUrl);
            yield return remoteResource;
            if (string.IsNullOrEmpty(remoteResource.error))
            {
                if (remoteResource.isDone)
                {
                    remoteResourceContrast = new ResourceContrast(remoteResource.text);
                    if (onfinsh != null)
                        onfinsh(isInit);
                }
            }
            else
            {
                Debug.LogError(remoteResource.error);
                if (onfinsh != null)
                    onfinsh(false);
            }

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
        string[] TryGetDepends(string assetbundleName)
        {
            string[] array = null;
            if (dependAssets.ContainsKey(assetbundleName))
            {
                array = dependAssets[assetbundleName];
            }
            else
            {
                array = manifest.GetAllDependencies(assetbundleName);
                dependAssets.Add(assetbundleName, array);
            }
            return array;
        }
      
        public void LoadAssetBundle(string assetbundleName, System.Action<AssetBundleItem> onfinsh)
        {
            string[] array = TryGetDepends(assetbundleName);
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
            var remoteInfo = remoteResourceContrast.GetContrastInfo(assetName);
            //var localInfo = localResourceContrast.GetContrastInfo(assetName);
            if (remoteInfo == null)
                Debug.LogError("************" + assetName);
            var expectLocalFile = GetLocalPath(remoteInfo.assetMd5);//预期存入到本地的文件
            string url = "";
            bool isLoadLocal = false;
            //if (localInfo != null)
            //{
            //    var localCacheFile = GetLocalPath(localInfo.assetMd5);//本地已经临时缓存的文件
            //    if (File.Exists(localCacheFile))//存在过期对象需要删除
            //    {
            //        File.Delete(localCacheFile);
            //    }
            //}

            if (File.Exists(expectLocalFile))
            {
                isLoadLocal = true;
                url = new System.Uri(expectLocalFile).AbsoluteUri;
            }
            else
            {
                url = Path.Combine(assetBundleRootUri, assetName);
            }
            WWW www = new WWW(url);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                if (www.isDone)
                {
                    var bundle = www.assetBundle;
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
                    var data = www.bytes;
                    if (!isLoadLocal)
                    {
                        DownLoadToLocal(remoteInfo.assetMd5, data);
                        ///将本地缓存的文件记录在比对文件中        
                        Debug.Log("Write: " + expectLocalFile);
                        File.AppendAllText(MainGameConfig.LocalResourcelistPath, remoteInfo.ToString());
                    }
                }

            }
            else
            {
                Debug.LogError(assetName + " :Load error " + www.error);
                if (onfinsh != null)
                {
                    onfinsh(assetName, false);
                }
                yield break;
            }
            www.Dispose();
        }

        void DownLoadToLocal(string assetName, byte[] data)
        {

            string filepath = GetLocalPath(assetName);
            FileStream fs = null;
            if (!File.Exists(filepath))
            {
                fs = File.Create(filepath);
            }
            else
            {
                fs = System.IO.File.OpenWrite(filepath);
            }

            fs.BeginWrite(data, 0, data.Length, result =>
            {
                var stream = result.AsyncState as System.IO.FileStream;
                try
                {
                    stream.EndWrite(result);
                    stream.Close();
                }
                catch (System.Exception ex)
                {
                    Debug.LogException(ex);
                }
            }, fs);       
        }
        public string GetLocalPath(string assetName)
        {
            string filepath = Path.Combine(MainGameConfig.LocalLuaPath, assetName);
            return filepath;
        }
        bool IsLoadedAllDepends(string assetName)
        {
            var array = TryGetDepends(assetName);
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

}