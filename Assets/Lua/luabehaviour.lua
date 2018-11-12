luabehaviour={}
---@param gameObject UnityEngine.GameObject
function luabehaviour:New(gameObject)
    local o = {}
    setmetatable(o,self)
    self.__index= self
    o.gameObject = gameObject
    print(o.gameObject.name,'New')
    return o
end
function luabehaviour:Awake()
    print( self.gameObject.name,'Awake')
end
function luabehaviour:Start()
    print(self.gameObject.name,'Start')
end
function luabehaviour:OnEnable()
    print(self.gameObject.name,'OnEnable')
end
function luabehaviour:OnDisable()
    print(self.gameObject.name,'OnDisable')
end
function luabehaviour:Update()
    print(self.gameObject.name,'Update')
end
