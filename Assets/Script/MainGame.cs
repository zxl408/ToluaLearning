using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour {
    public GameObject luaGo;
    NewTestEventListener listener;
    public string className;
    private void Awake()
    {
        listener= gameObject.AddComponent<NewTestEventListener>();
    }
    // Use this for initialization
    void Start () {
        (luaGo).AddComponent<LuaBaseBehaviour>().Init(className);
        var a = 1 << 2;
        print(a);
        var b = LayerMask.NameToLayer("Default");
        print("b:"+b);
        print("1<<b" + (1 << b));

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 80, 50), "clcik main")) {
            listener.OnClickEvent(gameObject);
        }
        if (GUI.Button(new Rect(20, 80, 80, 50), "clcik behaviour"))
        {
            listener.OnClickEvent(luaGo);
        }
    }
}
