using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildAssetbundleTool
{
    static string bundleTempDirectory = System.IO.Path.Combine(Application.dataPath, "../AssetBundles");
    static string resSeverPath = "E:/Res/resServer/TestLua";
#if UNITY_STANDALONE
    public static string osDir = "Win";
#elif UNITY_ANDROID
    public static string osDir = "Android";            
#elif UNITY_IPHONE
    public static string osDir = "iOS";        
#else
    public static string osDir = "";        
#endif
    [MenuItem("BuildAssetBundleTool/Build")]
    public static void Build()
    {
        Build(BuildAssetBundleOptions.None);
        Build(BuildAssetBundleOptions.UncompressedAssetBundle);
        Build(BuildAssetBundleOptions.ChunkBasedCompression);
    }
    public static void Build(BuildAssetBundleOptions options) {
        var targetDirectroy = Path.Combine(Path.Combine(bundleTempDirectory, osDir),options.ToString());
      
        if (!Directory.Exists(bundleTempDirectory))
        {
            Directory.CreateDirectory(bundleTempDirectory);
        }

        if (!Directory.Exists(targetDirectroy))
        {
            Directory.CreateDirectory(targetDirectroy);
            Debug.LogError(targetDirectroy);
        }
        CleanAllDestDir(targetDirectroy);
        MarkBundleNames(Path.Combine(Application.dataPath, "Game/Res"));
       
        BuildPipeline.BuildAssetBundles(targetDirectroy, options, EditorUserBuildSettings.activeBuildTarget);
    }
    //[MenuItem("GameObject/CreateGo %left u")]
    //static void CreateGo()
    //{
    //    Debug.LogError("CreateGo");
    //}
    //[MenuItem("GameObject/Create It Selected #p", false, 10)]
    //static void CreateIt(MenuCommand menuCommand)//MenuCommand 右键点出来的菜单上下文
    //{
    //    var go = GameObject.Instantiate(Selection.activeGameObject);
    //    go.name += " CreateIt";
    //    GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
    //    Undo.RegisterCreatedObjectUndo(go, "create " + go.name);
    //}
    //[MenuItem("GameObject/Create It Selected", true)]//必须与上面的函数一起用.不然没效果
    //static bool ValidateCreateIt()
    //{
    //    return Selection.activeGameObject != null;

    //}
    //[MenuItem("CONTEXT/Rigidbody/Do Something")]
    //static void DoSomething(MenuCommand command)
    //{
    //    Rigidbody body = (Rigidbody)command.context;
    //    body.mass = 5;
    //    Debug.Log("Changed Rigidbody's Mass to " + body.mass + " from Context Menu...");
    //}
    [MenuItem("BuildAssetBundleTool/Copy Bundle To ResSever")]
    public static void CopyBundleToResSever()
    {
        string origineDir = Path.Combine(bundleTempDirectory, osDir);
        if (!Directory.Exists(origineDir))
        {
            Debug.LogError(origineDir + " is not exists");
            return;
        }
        string detecDir = Path.Combine(resSeverPath, osDir);
        if (!Directory.Exists(detecDir))
        {
            Directory.CreateDirectory(detecDir);
        }
        CleanAllDestDir(detecDir);
        var list = new List<string>();
        GetChildDirectroys(origineDir, list);
        list.Insert(0,origineDir);//需要拷贝根目录下的文件
        foreach (var dir in list)
        {
            var files = Directory.GetFiles(dir);
            var destDir = dir.Replace(origineDir, detecDir);
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }
            foreach (var file in files)
            {
                File.Copy(file, file.Replace(origineDir, detecDir), true);
            }
        }
    }
    //删除目标目录下的所有文件包括目录
    public static void CleanAllDestDir(string detectDir)
    {
        if (!Directory.Exists(detectDir))
        {
            Debug.LogError(detectDir + " is not exists");
            return;
        }
        var list = new List<string>();
        GetChildDirectroys(detectDir, list);
        
        int count = list.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            var files = Directory.GetFiles(list[i]);
            foreach (var file in files)
            {
                File.Delete(file);
            }
            Directory.Delete(list[i]);
        }
        var child = Directory.GetFiles(detectDir);
        foreach (var file in child)
        {
            File.Delete(file);
        }
    }
    /// <summary>
    /// 获得所有子目录
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="list"></param>
    public static void GetChildDirectroys(string directory, List<string> list)
    {
        var array = Directory.GetDirectories(directory);
        int count = array.Length;
        for (int i = 0; i < count; i++)
        {
            var directroyName = array[i];
            list.Add(directroyName);
            GetChildDirectroys(directroyName, list);
        }
    }
    public static void MarkBundleNames(string directory)
    {
        List<string> directorys = new List<string>();
        GetChildDirectroys(directory, directorys);
        foreach (var path in directorys)
        {
            var array = Directory.GetFiles(path);
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                
                var fileWithPathName = array[i].Replace(Application.dataPath, "Assets");
               
                var importer = AssetImporter.GetAtPath(fileWithPathName);
                if (importer != null)
                {
                    var bundleMarkName = fileWithPathName.Replace("Assets\\", "").Split('.')[0] + ".unity3d";
                    importer.assetBundleName = bundleMarkName;
                }
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
