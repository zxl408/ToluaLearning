
local GameObject = UnityEngine.GameObject

local   ParticleSystem =UnityEngine.ParticleSystem
---@type UnityEngine.GameObject
local go = GameObject('go')
go:AddComponent(typeof(ParticleSystem))
go.transform:DORotate(Vector3(0,0,360), 2, DG.Tweening.RotateMode.FastBeyond360):OnComplete(OnComplete)
---@type UnityEngine.Transform
local t
UnityEngine.GameObject('dd')
