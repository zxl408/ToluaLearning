---
--- Generated by EmmyLua(https://github.com/EmmyLua)
--- Created by admin.
--- DateTime: 2018/11/30 13:25
---
LuaBaseBehaviourInjector={}
LuaBaseBehaviourInjector.Init=function()
    return function(self,a)
        print('*************Injector LuaBaseBehaviourInjector************'..a)
    end,LuaInterface.InjectType.After
end
InjectByModule(LuaBaseBehaviour,LuaBaseBehaviourInjector)