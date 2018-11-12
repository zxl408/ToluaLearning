using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour {
    public GameObject luaGo;
    NewTestEventListener listener;
    private void Awake()
    {
        listener= gameObject.AddComponent<NewTestEventListener>();
    }
    // Use this for initialization
    void Start () {
        (luaGo).AddComponent<LuaBaseBehaviour>().Init("luabehaviour");
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
