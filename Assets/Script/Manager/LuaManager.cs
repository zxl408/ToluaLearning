using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class LuaManager : LuaClient
{
    public LuaState LuaState
    {
        get
        {
            return luaState;
        }
    }
    protected override LuaFileUtils InitLoader()
    {
        var luafile = LuaFileUtils.Instance;
        luafile.beZip = true;//加上这个才能读取assetbundle
        return luafile;
    }

    protected override void OpenLibs()
    {
        base.OpenLibs();
        OpenCJson();
    }
    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();
#if ENABLE_LUA_INJECTION
#if UNITY_EDITOR
        if (UnityEditor.EditorPrefs.GetInt(Application.dataPath + "InjectStatus") == 1)
        {
#else
        if (true)
        {
#endif

        }
        else
#endif
        {
            Debug.LogError("查看是否开启了宏ENABLE_LUA_INJECTION并执行了菜单命令——\"Lua=>Inject All\"");
        }

    }
}


