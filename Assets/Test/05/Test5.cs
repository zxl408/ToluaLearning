using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test5 : MonoBehaviour {

    LuaState lua;
    //TestLua.lua.txt
    public TextAsset luaFile;
    // Use this for initialization
    void Start()
    {
        new LuaResLoader();
        lua = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);//绑定后lua脚本才能调用wrap文件
        DelegateFactory.Init();
        gameObject.AddComponent<LuaLooper>().luaState = lua;
        lua.DoString(luaFile.text);

    }

    // Update is called once per frame
    void Update()
    {
        lua.CheckTop();
        lua.Collect();
    }

    private void OnDisable()
    {
        lua.Dispose();//注意dispose不要乱加,最后放在disable里.以免lua脚本还没结束.释放就报错
        lua = null;
    }
}
