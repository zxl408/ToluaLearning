using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class LuaManager : LuaClient
{
    public LuaState LuaState {
        get {
            return luaState;
        }
    }
    protected override void OpenLibs()
    {
        base.OpenLibs();
        OpenCJson();
    }
}
