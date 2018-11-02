function foo()
    for i = 1, 10 do
        print(i)
        coroutine.wait(1)
    end
end

local co= coroutine.start(foo)
