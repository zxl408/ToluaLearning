
print('---------')
local color= Color.New(0,0,0,0)
print(color)


local function foo()
    for i = 1, 10 do
        print('time',i)
        coroutine.wait(1)
    end
    coroutine.step()
    local www= UnityEngine.WWW("www.baidu.com")
    coroutine.www(www)
    print(tolua.tolstring(www.bytes) )
end
coroutine.start(foo)