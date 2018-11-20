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