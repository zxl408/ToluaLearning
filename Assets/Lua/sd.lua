require("Luabehaviour")

local behaviour = {}

---@type this LuaBaseBehaviour
function behaviour:New(this)
    local o = Luabehaviour:New(this) or {}
    setmetatable(o, self)
    self.__index = self
    return o
end
function behaviour:Awake()
    Luabehaviour.Awake(self)
end
function behaviour:Start()
    Luabehaviour.Start(self)
end
function behaviour:OnEnable()
    Luabehaviour.OnEnable(self)
end
function behaviour:OnDisable()
    Luabehaviour.OnDisable(self)
end
function behaviour:Update()
    Luabehaviour.Update(self)
end
---@param go UnityEngine.GameObject
function behaviour:GetBehaviour(go)
   return Luabehaviour.GetBehaviour(self,go)
end
sd = behaviour
return behaviour