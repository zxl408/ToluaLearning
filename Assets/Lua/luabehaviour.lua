local behaviour={}
---@type LuaBaseBehaviour
local go
---@type TestEventListener
local listener

---@param this LuaBaseBehaviour
function behaviour:New(this)
    local o = {}
    setmetatable(o,self)
    self.__index= self
    o.this = this
    print(o.this.name,'New')
    return o
end
function behaviour:Awake()
    print( self.this.name,'Awake')

    go = UnityEngine.GameObject.Find('Main Camera')
    print(go.name)
    local type =typeof(NewTestEventListener)
    print(type)
    ---@type TestEventListener
    listener= go:GetComponent(type)
    print(listener.name)
    listener.onClickEvent= listener.onClickEvent+behaviour.OnStaticClick
end
function behaviour:Start()
    print(self.this.name,'Start')
end
function behaviour:OnEnable()
    print(self.this.name,'OnEnable')
end
function behaviour:OnDisable()
    listener.onClickEvent= listener.onClickEvent-behaviour.OnStaticClick
    print(self.this.name,'OnDisable')
end
function behaviour:OnDestroy()
    print(self.this .name,'OnDestroy')
end
function behaviour:Update()
    --print(self.this.name,'Update')
end
---@param go UnityEngine.GameObject
function behaviour:GetBehaviour(go)
    ---@type LuaBaseBehaviour
    local behaivor= go:GetComponent(typeof(LuaBaseBehaviour))
    if( behaivor ~= nil and behaivor.luaObj~=nil) then
        return behaivor
    else
        return nil
    end
end

---@param go UnityEngine.GameObject
function behaviour:OnClick(go)
    print( self.this.name, go.name)
end
---@param go UnityEngine.GameObject
function behaviour.OnStaticClick(go)
    ---@type LuaBaseBehaviour
    local behaivor = behaviour:GetBehaviour(go)
    if( behaivor ~= nil ) then
        behaviour.OnClick(behaivor.luaObj,go)
        print('click self')
    else
        print('click '..go.name)
    end
end


---@class behaviour
Luabehaviour = behaviour
setmetatable(behaviour,behaviour)
return behaviour