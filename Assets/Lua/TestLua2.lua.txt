function foo()
    WaitForSeconds(1)
    print(UnityEngine.Time.time)
    local www=UnityEngine.WWW('http://www.baidu.com')
    Yield(www)
    local s = tolua.tolstring(www.bytes)
    print(s:sub(1,129))
end
StartCoroutine(foo)
