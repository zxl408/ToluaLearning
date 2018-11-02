using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
//执行函数
public class Test3 : MonoBehaviour
{
    LuaState lua;
    string scriptStr = @"function sum(a)
                           return a 
                          end
test={}
test.func=sum
     ";
    LuaFunction luaFunc;
    // Use this for initialization
    void Start()
    {

        new LuaResLoader();
        lua = new LuaState();
        lua.Start();       
        DelegateFactory.Init();
        lua.DoString(scriptStr);
        //存储一个函数可以用来多次调用
        luaFunc = lua.GetFunction("test.func");
        //var function = lua.GetFunction("sum");
        if (luaFunc != null)
        {

            // 调用<T1, T2, R1> T 表示参数 R表示返回值
            int num = luaFunc.Invoke<int, int>(100);
            Debug.Log(string.Format("Invoke Number:{0}", num));


            num = Call(200);
            Debug.Log(string.Format("Call Number:{0}", num));

            //将函数转为委托 从而用委托调用 貌似只支持一个参数的 其他不支持的需要自己写
            System.Func<int, int> func = luaFunc.ToDelegate<System.Func<int, int>>();
            num = func(300);
            Debug.Log(string.Format("Delegate Number:{0}", num));


            //临时调用.不储存函数 适合频率较低的函数 调用<T1, T2, R1> T 表示参数 R表示返回值
            num = lua.Invoke<int, int>("test.func", 400, true);
            Debug.Log(string.Format("lua.Invoke{0}", num));
        }


        lua.CheckTop();
    }
    public int Call(int arg1)
    {
        luaFunc.BeginPCall();//开始调用函数
        luaFunc.Push(arg1);
        luaFunc.PCall();
        int number = (int)luaFunc.CheckNumber();
        luaFunc.EndPCall();
        return number;
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        if (luaFunc != null)
        {
            luaFunc.Dispose();
            luaFunc = null;
        }
        lua.Dispose();
        lua = null;
    }
}
