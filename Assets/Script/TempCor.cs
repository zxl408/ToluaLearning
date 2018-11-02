using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
public class TempCor : MonoBehaviour
{
    public TextAsset asset;
    LuaState lua;
	// Use this for initialization
	void Start () {
        new LuaResLoader();
        lua = new LuaState();
        lua.Start();
      
        LuaBinder.Bind(lua);//此方法可以用来访问wrap文件
        DelegateFactory.Init();//注册委托,以便于被lua文件识别调用
        var looper = gameObject.AddComponent<LuaLooper>();
        looper.luaState = lua;
        lua.DoString(asset.text);
      
      
	}


    // Update is called once per frame
    void Update () {
        lua.Collect();
    }
    private void OnDisable()
    {
        lua.Dispose();
    }
}
