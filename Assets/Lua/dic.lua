---@param dic System_Collections_Generic_KeyValuePair_int_TestAccountWrap
function foo(dic)

    print(getmetatable(dic))
    local it= dic:GetEnumerator()
    while it:MoveNext() do
        local v = it.Current.Value
        print("id: "..v.id.."name: "..v.name.."sex: "..v.sex)
    end
    local value
    isOk,value = dic:TryGetValue(1,nil)
    if isOk then
        print("Ok id: "..value.id.." name: "..value.name.." sex: "..value.sex)
    else
        print("bad number")
    end

    end