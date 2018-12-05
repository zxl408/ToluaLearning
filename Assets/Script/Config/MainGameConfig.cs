using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public static class MainGameConfig
{
   
#if UNITY_STANDALONE
    public static string osDir = "Win";
#elif UNITY_ANDROID
    public static string osDir = "Android";
#elif UNITY_IPHONE
    public static string osDir = "iOS";        
#else
    public static string osDir = "";        
#endif
    private static string remoteResUrl = "http://192.168.0.106:9200/resServer/TestLua";
    public static string RemoteResUri {
        get {
            
            System.Uri uri = new System.Uri(Path.Combine(remoteResUrl, osDir));
            return uri.AbsoluteUri;
        }
    }
    public static string LocalResPath
    {
        get
        {
            System.Uri uri = new System.Uri(Path.Combine(Application.persistentDataPath, osDir));
            return uri.AbsolutePath;
        }
    }
    private static string resourcelistUrl = "resourcelist.txt";
    public static string RemoteResourcelistUrl {
        get {

            System.Uri uri = new System.Uri(Path.Combine(RemoteResUri, resourcelistUrl));
            return uri.AbsoluteUri;
        }
    }
    public static string LocalResourcelistPath
    {
        get
        {
            System.Uri uri = new System.Uri(Path.Combine(LocalResPath, resourcelistUrl));
            return uri.AbsolutePath;
        }
    }
   
    private static string remoteLua = "Lua";
    public static string RemoteLuaUrl {
        get {         
            System.Uri uri = new System.Uri(Path.Combine(RemoteResUri, remoteLua));
            return uri.AbsoluteUri;
        }
    }
    private static string localLua = "Lua/";
    public static string LocalLuaPath
    {
        get
        {           
            System.Uri uri = new System.Uri(Path.Combine(LocalResPath, localLua));
            return uri.AbsolutePath;
        }
    }
    private static string uipath = "game/res/gui";
    public static string UIUrl {
        get {
            System.Uri uri = new System.Uri(Path.Combine(RemoteResUri, uipath));
            return uri.AbsoluteUri;
        }
    }
    public static string GetRelativeUIAssetName(string uiName) {
    
        return uipath+"/"+ uiName.ToLower() + ".unity3d";
    }

}
