using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
public class LuaStr : LuaClient {
 
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
    public string luaFile;
	// Use this for initialization
	void Start () {

      
    }

    private void Update()
    {
       
    }

    protected override void CallMain()
    {
        
    }
    protected override void OnLoadFinished()
    {
        base.OnLoadFinished();
        luaState.DoFile(luaFile);
    }

}
