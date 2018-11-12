using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
public enum TestLuaEnum {
    One,
    Two,
    Three,
    Four
}
public class SelfTestEnum : MonoBehaviour {
    public LuaState luaState;
    public string fileName = "TestEnum";
    private void Awake()
    {
        luaState = new LuaState();
        LuaBinder.Bind(luaState);
        luaState.Start();
    }
    // Use this for initialization
    void Start () {
        TestLuaEnum luaEnum = TestLuaEnum.Three;
        luaState.DoFile(fileName);
        var func= luaState.GetFunction("TestEnum");
        func.Call(luaEnum);
	}
	
	// Update is called once per frame
	void Update () {
        luaState.Collect();
        luaState.CheckTop();
	}
    private void OnDestroy()
    {
        luaState.Dispose();
        luaState = null;
    }
}
