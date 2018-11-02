using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;//tolua命名空间
//执行字符串
public class Test : MonoBehaviour {
    LuaState lua;//lua虚拟机
	// Use this for initialization
	void Start () {
        lua = new LuaState();//创建lua虚拟机
        lua.Start();//初始化虚拟机
        lua.DoString("print('hello tolua')","Test.cs");//执行lua语句
        lua.CheckTop();//检查栈顶是否为空
        lua.Dispose();//析构函数 释放内存
        lua = null;//赋空 优化内存
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
