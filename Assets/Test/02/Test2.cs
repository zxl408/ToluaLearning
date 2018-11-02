using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using System.IO;
//直接执行文件
public class Test2 : MonoBehaviour {
    public Rect rect1 = new Rect(new Vector2(100,100),new Vector2(100,50));
    public Rect rect2 = new Rect(new Vector2(100, 200), new Vector2(100, 50));
    public string filePath;
    LuaState lua;
    // Use this for initialization
    void Start () {
        filePath = Path.Combine(Application.dataPath, "Test");
        lua = new LuaState();
        lua.Start();
        lua.AddSearchPath(filePath);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
        if (GUI.Button(rect1, "print1")) {
            lua.DoFile("test2.lua");
        }
        if (GUI.Button(rect2, "print2"))
        {
            lua.Require("test2");
        }
        lua.Collect();//垃圾回收 对于自动gc的 Luafunction,LuaTable,以及委托减去的Luafuntion,延迟删除的object类.等等延迟处理的回都在这里自动处理
        lua.CheckTop();//检查是否堆栈平衡,一般放于update中.c#中任何使用堆栈炒作,都需要调用者自己平衡堆栈.出现警告时,自己检查代码
    }
    private void OnDestroy()
    {
      
        lua.Dispose();
        lua = null;
    }
}
