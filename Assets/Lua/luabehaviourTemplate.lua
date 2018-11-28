---Awake OnEnable... 这些函数需要都写上.不然要报错
require("Luabehaviour")

local json = require 'cjson'
local behaviour = {}
local base = Luabehaviour
local box = UnityEngine.BoxCollider --需要加上这个完成预加载BoxCollider(UnityEngine.Physics.Raycast 需要)
---@param this LuaBaseBehaviour
function behaviour:New(this)
    local o = base:New(this) or {}
    setmetatable(o, self)
    self.__index = self
    return o
end
function behaviour:Awake()
    base.Awake(self)
end
function behaviour:Start()
    base.Start(self)
    --instantiation class
    --t=TestClassA()
    --t:Start()
    --19 cjson and io
    --local text=''
    --for line in io.lines('D:/UnityWorkSpace/TestWorkSpace/tolua-master/Assets/ToLua/Examples/Resources/jsonexample.json') do
    --  text= text..line
    --end
    --print(text)
    --local data = json.decode(text)
    --print(data.glossary.title)
    --s = json.encode(data)
    --print(s)
    ---@type LuaBaseBehaviour
    local behaviour=self.this
    behaviour.enableUpdateFunc = true

    local a = int64.new(1,2)
    print(tostring(a),tolua.typename(a))
    local c=18.057+20.0088
    print(c,tolua.typename(c))
    --20 utf8
    --local utf8 = utf8
    --local n1= utf8.len('你好伙计')
    --local n2=utf8.len('こんにちは')
    --
    --local s='遍历字符串hhh'
    --print(s:sub(1,3))
    --print(s:find('遍历'))
    --print(s:sub(4,nil))--为nil表示后面所有
    --for i in utf8.byte_indices(s) do
    --
    --    local next = utf8.next(s,i)
    --    local it =next and next -1
    --
    --    print(s:sub(i,next and next -1))
    --
    --end
    --str = "天下风云我辈出"
    --print("风云 count ".. utf8.count(str,'风云'))
    --print(" count ".. utf8.count(str,'风云'))
    --str = str:gsub('风云','風雲')
    --print(str)
    --print("n1 ".. n1);
    --print("n2 ".. n2);
    --local  function replace(s,i,j,repl_char)
    --    if s:sub(i,j) == '辈' then
    --        return repl_char
    --    end
    --end
    --print(utf8.replace(str,replace,'輩'))

     ----21 string
    -- str = System.String.New('Nihao')--创建c#里的string类 类型为userdata
    -- print("str type "..type(str))
    -- index= str:IndexOf('Ni')
    -- print(index)
    -- local buffer = str:ToCharArray()
    -- print("buffer type "..type(buffer))
    --
    -- for i=0, buffer.Length-1  do
    --     print(string.char(buffer[i]))
    -- end
    --local str1 = tolua.tolstring(buffer)--将c#char[]类型转换为lua里的string
    -- print("str1 "..str1.." type "..type(str1))
    --local luastr= tolua.tolstring(str)--tolstring 里面的类型不能为lua里的string类型 (如: local luastr= tolua.tolstring(str1) 这是错误的)
    -- print("luastr "..luastr.." type "..type(luastr))

    ----Reflection 22
    require 'tolua.reflection'
    tolua.loadassembly('Assembly-CSharp')
    local BidngFlags = require('System.Reflection.BindingFlags')
    local t = typeof('TestExport')
    local func = tolua.getmethod(t,'TestReflection')
    func:Call()
    func:Destroy()

    local objs = {Vector3.one,Vector3.zero}
    local array = tolua.toarray(objs,typeof(Vector3))
    local obj = tolua.createinstance(t,array)


end
function behaviour:OnEnable()
    base.OnEnable(self)
end
function behaviour:OnDisable()
    base.OnDisable(self)
end
function behaviour:OnDestroy()
    base.OnDestroy(self)
end
function behaviour:Update()
    base.Update(self)
    --14 out
    --if(UnityEngine.Input.GetMouseButton(0)) then
    --
    --    local ray = UnityEngine.Camera.main:ScreenPointToRay(UnityEngine.Input.mousePosition)
    --    local layer=2^LayerMask.NameToLayer('Default')
    --    local flag,hit= UnityEngine.Physics.Raycast(ray,nil,5000,layer)
    --    if(flag) then
    --        print(hit.point.x,hit.point.y,hit.point.z,hit.transform.name)
    --    end
    --end

end
---@param go UnityEngine.GameObject
function behaviour:GetBehaviour(go)
    return base.GetBehaviour(self, go)
end
luabehaviourTemplate = behaviour
return behaviour