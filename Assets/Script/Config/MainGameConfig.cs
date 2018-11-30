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
    public static string RemoteResUrl {
        get {
            
            System.Uri uri = new System.Uri(Path.Combine(remoteResUrl, osDir));
            return uri.AbsoluteUri;
        }
    }

    public static string LocalResUrl
    {
        get
        {
            System.Uri uri = new System.Uri(Path.Combine(Application.persistentDataPath, osDir));
            return uri.AbsoluteUri;
        }
    }
    private static string remoteLuaUrl = "Lua";
    public static string RemoteLuaUrl {
        get {         
            System.Uri uri = new System.Uri(Path.Combine(RemoteResUrl, remoteLuaUrl));
            return uri.AbsoluteUri;
        }
    }
    private static string localLuaUrl = "Lua/";
    public static string LocalLuaUrl
    {
        get
        {           
            System.Uri uri = new System.Uri(Path.Combine(LocalResUrl, localLuaUrl));
            return uri.AbsoluteUri;
        }
    }
}
