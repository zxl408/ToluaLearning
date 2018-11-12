using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
public class Test8 : MonoBehaviour {
    LuaState lua;
    public string assetName;
	// Use this for initialization
	void Start () {
        lua = new LuaState();
        lua.Start();
        lua.DoFile(assetName);
        var fun = lua.GetFunction("foo");
        int[] array = new int[] { 1, 3, 5, 7, 9 };
        fun.BeginPCall();
        fun.Push(array);
        fun.PCall();
        //check函数必须放在endcall前面 注意check顺序必须和返回值顺序一致
        var n = fun.CheckNumber();        
        var s = fun.CheckString();
        var b = fun.CheckBoolean();
        fun.EndPCall();
       
        Debug.LogError(string.Format("n:{0} b:{1} s:{2}", n, b, s));
	}
	
	// Update is called once per frame
	void Update () {
        lua.CheckTop();
        lua.Collect();
	}
    private void OnDestroy()
    {
        lua.Dispose();
        lua = null; 
    }
}
