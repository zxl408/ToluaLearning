using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaInterface;
public class Test4 : MonoBehaviour
{
    string script = @" 
  
       print('hello: '..strName)  
       table={1,2,3,4}
       table.name='tableName'
        
      
       table1={
	        name='zxl',	    
	        id=1
        }
        print(#table.name)
        for k, v in pairs(table1) do
            print(k.. ' - ' .. v)
        end

       local i =0
       repeat         
         print(i)
          i=i+1
       until(i>10)


       meta={name='metaName'}
       setmetatable(table,meta)
       print(type(a))
       
       print('-----------')
       function testFun(tab,fun)
           for k,v in ipairs(tab) do
                 print(fun(k,v))
           end

        end
    
       testFun(table,function(k,v)
          if(v == nil)then
          return nil
          end
          return k..'-'..v  --连接的字符串只能是字符串和数字
        end);

       table.map={name='mapName10',id=10}

";

    LuaState lua;
    // Use this for initialization
    void Start()
    {
        lua = new LuaState();
        lua.Start();
        lua["strName"] = "luaZxl";
        lua.DoString(script, "Test4");
        LuaTable luaTable = lua.GetTable("table");
        Debug.Log(string.Format("table Name:{0}", luaTable["name"]));
        var luaTable1 = luaTable.GetTable<LuaTable>("map");
        Debug.Log(string.Format("table Map Name:{0}", luaTable1["name"]));

        luaTable1["name"] = "modify";
        Debug.Log(string.Format("table modify Map Name:{0}", luaTable1["name"]));


        luaTable.AddTable("newRow");
        luaTable.SetTable("newRow", "new Row");
        Debug.Log(string.Format("table newRow:{0}", luaTable["newRow"]));
        luaTable1.Dispose();
        luaTable1 = luaTable.GetMetaTable();

        var array = luaTable.ToArray();
        for (int i = 0; i < array.Length; i++)
        {
            Debug.Log(string.Format("{0}:{1}", i, array[i]));
        }

        Debug.Log(string.Format("table MetaTable:{0}", luaTable1["name"]));
        lua.CheckTop();
        lua.Collect();
        lua.Dispose();
        lua = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
