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
        var origin = Path.Combine(Application.dataPath, "Game/Res");
        var targetDirectroy = Path.Combine(Path.Combine(bundleTempDirectory, osDir), BuildAssetBundleOptions.None.ToString());
        Build(BuildAssetBundleOptions.None, origin, targetDirectroy);
        targetDirectroy = Path.Combine(Path.Combine(bundleTempDirectory, osDir), BuildAssetBundleOptions.UncompressedAssetBundle.ToString());
        Build(BuildAssetBundleOptions.UncompressedAssetBundle, origin, targetDirectroy);
        targetDirectroy = Path.Combine(Path.Combine(bundleTempDirectory, osDir), BuildAssetBundleOptions.ChunkBasedCompression.ToString());
        Build(BuildAssetBundleOptions.ChunkBasedCompression, origin, targetDirectroy);
    }

    [MenuItem("BuildAssetBundleTool/BuildLevel(打包场景模式))")]
    public static void BuildScenceLevel()
    {
        var targetDirectroy = Path.Combine(Path.Combine(bundleTempDirectory, osDir), "Scenes");

        var origin = Path.Combine(Application.dataPath, "Scene");


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
        var list = MarkBundleNames(origin);

        BuildPipeline.BuildAssetBundles(targetDirectroy, list.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        Caching.CleanCache();
    }
    public static void Build(BuildAssetBundleOptions options, string origin, string targetDirectroy)
    {

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
        MarkBundleNames(origin);

        BuildPipeline.BuildAssetBundles(targetDirectroy, options, EditorUserBuildSettings.activeBuildTarget);
        Caching.CleanCache();
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
        GetAllDirectroys(origineDir, list);
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
        GetAllDirectroys(detectDir, list);

        int count = list.Count;
        for (int i = count - 1; i >= 1; i--)//不删除根目录
        {
            var files = Directory.GetFiles(list[i]);
            foreach (var file in files)
            {
                File.Delete(file);
            }
            Directory.Delete(list[i]);
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
    /// <summary>
    /// 获得所有子目录(包含自身)
    /// </summary>
    /// <param name="directory"></param>
    /// <param name="list"></param>
    public static void GetAllDirectroys(string directory, List<string> list)
    {
        GetChildDirectroys(directory, list);
        list.Insert(0, directory);
    }
    public static List<AssetBundleBuild> MarkBundleNames(string directory)
    {
        List<string> directorys = new List<string>();
        List<AssetBundleBuild> builds = new List<AssetBundleBuild>();
        GetAllDirectroys(directory, directorys);
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



                    AssetBundleBuild build = new AssetBundleBuild();
                    build.assetBundleName = importer.assetBundleName;
                    //build.assetBundleVariant = importer.assetBundleVariant;
                    var assetName = Path.GetFullPath(array[i]);
                    assetName = assetName.Replace("\\", "/");
                    assetName = assetName.Replace(Application.dataPath, "Assets");
                    build.assetNames = new string[1] { assetName };

                    builds.Add(build);
                }
            }
        }
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return builds;
    }
}
