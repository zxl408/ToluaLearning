using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
public class DicTest : MonoBehaviour {
    Dictionary<int, TestAccount> dic = new Dictionary<int, TestAccount>();
    LuaState lua;
    public TextAsset asset;
	// Use this for initialization
	void Start () {

        new LuaResLoader();
        dic.Add(1, new TestAccount(1, "zxl1", 0));
        dic.Add(2, new TestAccount(2, "zxl2", 0));
        dic.Add(3, new TestAccount(3, "zxl3", 0));
        dic.Add(4, new TestAccount(4, "zxl4", 0));

        lua = new LuaState();
        lua.Start();
         BindMap(lua);
        lua.DoString(asset.text);
        var func = lua.GetFunction("foo");
     
        func.Call(dic);


    }

    //示例方式，方便删除，正常导出无需手写下面代码
    void BindMap(LuaState L)
    {
        L.BeginModule(null);
        TestAccountWrap.Register(L);
        L.BeginModule("System");
        L.BeginModule("Collections");
        L.BeginModule("Generic");
        System_Collections_Generic_Dictionary_int_TestAccountWrap.Register(L);
        System_Collections_Generic_KeyValuePair_int_TestAccountWrap.Register(L);
        L.BeginModule("Dictionary");
        System_Collections_Generic_Dictionary_int_TestAccount_KeyCollectionWrap.Register(L);
        System_Collections_Generic_Dictionary_int_TestAccount_ValueCollectionWrap.Register(L);
        L.EndModule();
        L.EndModule();
        L.EndModule();
        L.EndModule();
        L.EndModule();
    }
    // Update is called once per frame
    void Update () {
        lua.Collect();
        lua.CheckTop();
	}
    private void OnDestroy()
    {
        lua.Dispose();
        lua = null;
    }
}
