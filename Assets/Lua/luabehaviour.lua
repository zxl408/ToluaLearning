luabehaviour={}
---@type LuaBaseBehaviour
local go
---@type TestEventListener
local listener
---@type System.Collections.Generic.Dictionary_2_TKey_TValue_
local dc


---@param this LuaBaseBehaviour
function luabehaviour:New(this)
    local o = {}
    setmetatable(o,self)
    self.__index= self
    o.this = this
    o.dc =
    print(o.this.name,'New')
    return o
end
function luabehaviour:Awake()
    print( self.this.name,'Awake')

    go = UnityEngine.GameObject.Find('Main Camera')
    print(go.name)
    local type =typeof(NewTestEventListener)
    print(type)
    ---@type TestEventListener
    listener= go:GetComponent(type)
    print(listener.name)
    listener.onClickEvent= listener.onClickEvent+luabehaviour.OnStaticClick
end
function luabehaviour:Start()
    print(self.this.name,'Start')
end
function luabehaviour:OnEnable()
    print(self.this.name,'OnEnable')
end
function luabehaviour:OnDisable()
    listener.onClickEvent= listener.onClickEvent-luabehaviour.OnStaticClick
    print(self.this.name,'OnDisable')
end
function luabehaviour:Update()
    print(self.this.name,'Update')
end
---@param go UnityEngine.GameObject
function luabehaviour:OnClick(go)
    print( self. go.name)
end
---@param go UnityEngine.GameObject
function luabehaviour.OnStaticClick(go)
    if(go == ) then
        OnClick(go)
        print('click self')
    end
end
