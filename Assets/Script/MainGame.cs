using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour {
    public GameObject luaGo;
	// Use this for initialization
	void Start () {
        (luaGo).AddComponent<LuaBaseBehaviour>().Init("luabehaviour");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
