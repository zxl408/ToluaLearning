using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadAssetBundleTest : MonoBehaviour {
     string uri = "http://192.168.0.106:9200/resServer/TestLua/Win/game/res/model/cube.unity3d";
	// Use this for initialization
	void Start () {
        //StartCoroutine(LoadAssetBundle());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 60, 30), "start")) {
            StartCoroutine(LoadAssetBundle());
        }
    }
    IEnumerator LoadAssetBundle() {
        float startTime = Time.time;

        UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.GetAssetBundle(uri,0);
      
        yield return request.Send();
        AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);
        
        float duration = Time.time - startTime;
        startTime = Time.time;
        print("download from uri time:"+duration);
        yield return bundle.LoadAllAssetsAsync();
        duration = Time.time - startTime;
        print("Load from assetbundle time:" + duration);
    }
}
