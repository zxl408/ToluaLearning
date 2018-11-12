using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
/// <summary>
/// 物体挂载lua脚本
/// </summary>
public class LuaBaseBehaviour : MonoBehaviour
{
    public LuaTable luaClass;
    public LuaTable luaObj;
    public string className;
    private LuaState luaState
    {
        get
        {
            return LuaManager.Instance.luaState;
        }
    }
    static LuaFunction newFunc;
    static LuaFunction awakeFunc;
    static LuaFunction startFunc;
    static LuaFunction enableFunc;
    static LuaFunction disableFunc;
    static LuaFunction updateFunc;
    public bool enableUpdateFunc = false;
    public bool isInit
    {
        get;
        protected set;
    }

    private void Awake()
    {
        if (isInit)
            awakeFunc.Call(luaObj);
        print("Awake");
    }
    public void Init(string className)
    {
        this.className = className;
        luaState.Require(className);

        var luaClass = luaState.GetTable(className);
        newFunc = luaClass.GetLuaFunction("New");
        if (newFunc != null)
        {
            newFunc.BeginPCall();
            newFunc.Push(luaClass);
            newFunc.Push(gameObject);
            newFunc.PCall();
            luaObj = newFunc.CheckLuaTable();
            newFunc.EndPCall();
        }

        if (luaObj != null)
            isInit = true;
        print("Init");
        if (isInit)
        {
            if (awakeFunc == null)
                awakeFunc = luaClass.GetLuaFunction("Awake");
            if (startFunc == null)
                startFunc = luaClass.GetLuaFunction("Start");
            if (enableFunc == null)
                enableFunc = luaClass.GetLuaFunction("OnEnable");
            if (disableFunc == null)
                disableFunc = luaClass.GetLuaFunction("OnDisable");
            if (updateFunc == null)
                updateFunc = luaClass.GetLuaFunction("Update");

            if (isActiveAndEnabled)
            {
                awakeFunc.Call(luaObj);
                enableFunc.Call(luaObj);
            }

        }

    }


    // Use this for initialization
    void Start()
    {
        print("Start");
        if (isInit)
            startFunc.Call(luaObj);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit && enableUpdateFunc)
            updateFunc.Call(luaObj);
    }
    private void OnEnable()
    {
        print("OnEnable");
        if (isInit)
            enableFunc.Call(luaObj);
    }
    private void OnDisable()
    {
        print("OnDisable");
        if (isInit)
            disableFunc.Call(luaObj);
    }
    private void OnDestroy()
    {
        luaObj.Dispose();
        luaObj = null;
    }
}
