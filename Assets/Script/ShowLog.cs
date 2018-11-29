using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLog : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        style.fontSize = 60;
        Application.logMessageReceived += ShowTips;
        DontDestroyOnLoad(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnApplicationQuit()
    {
#if UNITY_5 || UNITY_2017 || UNITY_2018
        Application.logMessageReceived -= ShowTips;
#else
        Application.RegisterLogCallback(null);
#endif
    }

    string tips = null;

    void ShowTips(string msg, string stackTrace, LogType type)
    {
        tips += msg;
        tips += "\r\n";
    }
    GUIStyle style = new GUIStyle();
 

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width / 2 -100, Screen.height / 2 - 00, 600, 400), tips, new GUIStyle());
    }
}
