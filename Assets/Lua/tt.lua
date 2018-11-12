
local GameObject = UnityEngine.GameObject

local   ParticleSystem =UnityEngine.ParticleSystem

local type = UnityEngine.Space.IntToEnum(0)
print(type)
for i, v in ipairs(UnityEngine.Space) do
print(i,v)
end


--type = UnityEngine.PrimitiveType.IntToEnum(0)
--print(type)
--print(getmetatable(UnityEngine.PrimitiveType))



--UnityEngine.PrimitiveType={'Sphere''Capsule',"Cylinder","Cube",'Plane','Quad'}





---@type UnityEngine.GameObject
local go = UnityEngine.GameObject.CreatePrimitive(UnityEngine.PrimitiveType.IntToEnum(0))
go:AddComponent(typeof(ParticleSystem))
go.transform:DORotate(Vector3(0,0,360), 2, DG.Tweening.RotateMode.FastBeyond360):OnComplete(OnComplete)
---@type DelegateFactory.DG_Tweening_Core_DOGetter_float_Event

t={}
DG.Tweening.DOTween.To(DG.Tweening.Core.DOGetter_float(function ()
    return 10
    end),DG.Tweening.Core.DOSetter_float(function (a)
 go.transform.position = Vector3.New(0,a,0)
end),100,10)
---@type UnityEngine.Transform
local t
UnityEngine.GameObject('dd')
