using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
using UnityEngine.SceneManagement;

public class LuaManager : MonoBehaviour
{
    public LuaState luaState;
    public LuaLooper looper;
    public LuaResLoader loder;
    public static LuaManager Instance
    {
        get;
        protected set;
    }

    private void Awake()
    {
        Instance = this;
        name = "LuaManager";
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (luaState != null)
            luaState.RefreshDelegateMap();
    }

    // Use this for initialization
    void Start()
    {
        Init();

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        looper.Destroy();
        looper = null;
        luaState.Dispose();
        luaState = null;

    }
    void StartLooper()
    {
        looper = gameObject.AddComponent<LuaLooper>();
        looper.luaState = luaState;
    }
    void Init()
    {
        loder = new LuaResLoader();
        luaState = new LuaState();

        luaState.LuaSetTop(0);
        Bind();
        OnLoadFileFinsh();

    }
    void Bind()
    {
        LuaBinder.Bind(luaState);
        DelegateFactory.Init();
        LuaCoroutine.Register(luaState, this);
    }
    public virtual void OnLoadFileFinsh()
    {
        luaState.Start();
        StartLooper();
    }
}
