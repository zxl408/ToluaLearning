using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundleTest : MonoBehaviour
{
    //string formatUri = "http://192.168.0.106:9200/resServer/TestLua/Win/{0}/game/res/model/cube.unity3d";
    //string formatUri = "http://192.168.0.106:9200/resServer/TestLua/Win/{0}/game/res/gui/mainui.unity3d";
    string formatUri = "http://192.168.0.106:9200/resServer/TestLua/Win/{0}/game/res/temp.unity3d";
    //string formatUri = "file://E:/Res/resServer/TestLua/Win/{0}/game/res/model/cube.unity3d";
    // Use this for initialization
    void Start()
    {
        //StartCoroutine(LoadAssetBundle());

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 30), "None"))
        {
            StartCoroutine(LoadAssetBundle(UnityEditor.BuildAssetBundleOptions.None));
        }
        if (GUI.Button(new Rect(10, 60, 200, 30), "UncompressedAssetBundle"))
        {
            StartCoroutine(LoadAssetBundle(UnityEditor.BuildAssetBundleOptions.UncompressedAssetBundle));
        }
        if (GUI.Button(new Rect(10, 110, 200, 30), "ChunkBasedCompression"))
        {
            StartCoroutine(LoadAssetBundle(UnityEditor.BuildAssetBundleOptions.ChunkBasedCompression));
        }
    }
    //IEnumerator LoadAssetBundle(UnityEditor.BuildAssetBundleOptions options)
    //{
    //    var uri = string.Format(formatUri, options);
    //    float startTime = Time.time;

    //    UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(uri, 0);

    //    yield return request.Send();
    //    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

    //    float duration = Time.time - startTime;
    //    startTime = Time.time;
    //    print(string.Format("download {0} with type:{1} time:{2}", bundle.name, options, duration));
    //    //var request1 = bundle.LoadAssetAsync("01");
    //    //yield return request1;
    //    //if (request1.isDone)
    //    //{
    //    //    duration = Time.time - startTime;
    //    //    startTime = Time.time;
    //    //    print(string.Format("Load {0} with type:{1} time: {2}", request1.asset.name, options, duration));
    //    //    bundle.Unload(false);
    //    //}
    //    //request.Dispose();

    //    // request1 = bundle.LoadAssetAsync("02");
    //    //yield return request1;
    //    //if (request1.isDone)
    //    //{
    //    //    duration = Time.time - startTime;
    //    //    startTime = Time.time;
    //    //    print(string.Format("Load {0} with type:{1} time: {2}", request1.asset.name, options, duration));
    //    //    bundle.Unload(false);
    //    //}

    //    var request1 = bundle.LoadAssetAsync("02");
    //    yield return request1;
    //    if (request1.isDone)
    //    {
    //        duration = Time.time - startTime;
    //        startTime = Time.time;
    //        print(string.Format("Load {0} with type:{1} time: {2}", request1.asset.name, options, duration));
    //        bundle.Unload(false);
    //    }
    //    request.Dispose();

    //}
    //IEnumerator LoadAssetBundle(UnityEditor.BuildAssetBundleOptions options)
    //{
    //    var uri = string.Format(formatUri, options);
    //    float startTime = Time.time;

    //    UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(uri, 0);

    //    yield return request.Send();
    //    AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
    //    float duration = Time.time - startTime;
    //    startTime = Time.time;
    //    print(string.Format("download {0} with type:{1} time:{2}", bundle.name, options, duration));
    //    var request1 = bundle.LoadAllAssetsAsync();
    //    yield return request1;
    //    if (request1.isDone)
    //    {
    //        duration = Time.time - startTime;
    //        print(string.Format("Load {0} with type:{1} time: {2}", bundle.name, options, duration));
    //        Instantiate(request1.asset);
    //        bundle.Unload(false);
    //    }
    //    request.Dispose();               
    //}
    IEnumerator LoadAssetBundle(UnityEditor.BuildAssetBundleOptions options)
    {
        var uri = string.Format(formatUri, options);
        float startTime = Time.time;
        string manifestUri = string.Format("http://192.168.0.106:9200/resServer/TestLua/Win/{0}/{0}", options);
        UnityEngine.Networking.UnityWebRequest reqManifest = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(manifestUri, 0);

        yield return reqManifest.Send();
        var manifestAssetbundle = DownloadHandlerAssetBundle.GetContent(reqManifest);
        var manifest = manifestAssetbundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        if (manifest != null)
        {
            foreach (var it in manifest.GetDirectDependencies("game/res/gui/mainui.unity3d"))
                print(it);
            foreach (var it in manifest.GetAllAssetBundles())
                Debug.Log(it);
        }
        else
        {
            Debug.Log("manifest == null");
        }
        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(uri, 0);

        yield return request.Send();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

        float duration = Time.time - startTime;
        startTime = Time.time;
        print(string.Format("download {0} with type:{1} time:{2}", bundle.name, options, duration));


        duration = Time.time - startTime;
        print(string.Format("Load {0} with type:{1} time: {2}", bundle.name, options, duration));

        request.Dispose();
    }
}
