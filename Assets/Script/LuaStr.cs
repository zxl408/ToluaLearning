using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
public class LuaStr : MonoBehaviour {
    LuaState lua;
    string script = @"
          local str1='abac0 (hello123) &*e'
           print('*'..string.match('*******','*+'))
          print('match empty '..string.match(str1,'%a'))
          print('match * '..string.match(str1,'%a*'))
          print('match + '..string.match(str1,'%a+'))
          print('match - '..string.match(str1,'%a-'))
          print('match ? '..string.match(str1,'%a?'))
          print('match %f[] '..string.match(str1,'%f[a]'))
          print('match %b '..string.match(str1,'%b()'))
          print('match ^ '..string.match(str1,'^%a+'))
          print(string.match(str1,'%a+$'))
          print(string.gsub(str1,'^%s*(.-)%s*$','%1'))
          print(string.gsub('flaaap','()aa()','%1'))
          print(string.gsub('flaaap','()aa()','%2'))
          print(string.gsub('flaaap','(%a)aa(%a)','%2'))
";
    public TextAsset luaFile;
	// Use this for initialization
	void Start () {
        new LuaResLoader();
        lua = new LuaState();
        lua.Start();
        LuaBinder.Bind(lua);//绑定后lua脚本才能调用wrap文件
        DelegateFactory.Init();
        gameObject.AddComponent<LuaLooper>().luaState = lua;
        lua.DoString(luaFile.text);
        
    }
	
	// Update is called once per frame
	void Update () {
        lua.CheckTop();
        lua.Collect();
	}

    private void OnDisable()
    {
        lua.Dispose();//注意dispose不要乱加,最后放在disable里.以免lua脚本还没结束.释放就报错
        lua = null;
    }
}
