using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test6 : LuaClient {
    //TestLua2.lua.txt
    public TextAsset asset;

    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();
        luaState.DoString(asset.text);
    }
}
