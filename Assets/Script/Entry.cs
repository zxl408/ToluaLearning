using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zxl.Res;
public class Entry : MonoBehaviour
{
    ResManager ResManager;
    // Use this for initialization
    void Start()
    {
        ResManager = ResManager.GetResManager();
        ResManager.InitRes((isSuceess) =>
        {
            if (isSuceess)
            {
                var luaManager = (new GameObject("LuaManager")).AddComponent<LuaManager>();
                DontDestroyOnLoad(luaManager);
                UnityEngine.SceneManagement.SceneManager.LoadScene("mainGame");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
  
}
