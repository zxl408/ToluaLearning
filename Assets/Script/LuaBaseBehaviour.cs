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
            return ((LuaManager)LuaManager.Instance).LuaState;
        }
    }
    static LuaFunction newFunc;
    static LuaFunction awakeFunc;
    static LuaFunction startFunc;
    static LuaFunction enableFunc;
    static LuaFunction disableFunc;
    static LuaFunction destroyFunc;
    static LuaFunction updateFunc;
    public bool enableUpdateFunc = false;
    public bool isInit
    {
        get;
        protected set;
    }

    private void Awake()
    {
        if (isInit && awakeFunc != null)
            awakeFunc.Call(luaObj);
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
            newFunc.Push(this);
            newFunc.PCall();
            luaObj = newFunc.CheckLuaTable();
            newFunc.EndPCall();
        }

        if (luaObj != null)
            isInit = true;
        if (isInit)
        {
            if (awakeFunc == null)
                awakeFunc = luaObj.GetLuaFunction("Awake");
            if (startFunc == null)
                startFunc = luaObj.GetLuaFunction("Start");
            if (enableFunc == null)
                enableFunc = luaObj.GetLuaFunction("OnEnable");
            if (disableFunc == null)
                disableFunc = luaObj.GetLuaFunction("OnDisable");
            if (destroyFunc == null)
                destroyFunc = luaObj.GetLuaFunction("OnDestroy");
            if (updateFunc == null)
                updateFunc = luaObj.GetLuaFunction("Update");

            if (isActiveAndEnabled)
            {
                if (awakeFunc != null)
                    awakeFunc.Call(luaObj);
                if (enableFunc != null)
                    enableFunc.Call(luaObj);
            }

        }

    }


    // Use this for initialization
    void Start()
    { 
        if (isInit && startFunc != null)
            startFunc.Call(luaObj);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInit && enableUpdateFunc && updateFunc != null)
            updateFunc.Call(luaObj);
    }
    private void OnEnable()
    { 
        if (isInit && enableFunc != null)
            enableFunc.Call(luaObj);
    }
    private void OnDisable()
    {      
        if (isInit && disableFunc != null)
            disableFunc.Call(luaObj);
    }
    private void OnDestroy()
    {
        if (luaObj != null)
        {         
            if (isInit && destroyFunc != null)
                destroyFunc.Call(luaObj);
            luaObj.Dispose();
            luaObj = null;
            awakeFunc.Dispose();
            awakeFunc = null;
            startFunc.Dispose();
            startFunc = null;
            enableFunc.Dispose();
            enableFunc = null;
            disableFunc.Dispose();
            disableFunc = null;
            destroyFunc.Dispose();
            destroyFunc = null;
            updateFunc.Dispose();
            updateFunc = null; 
        }
    }
}
