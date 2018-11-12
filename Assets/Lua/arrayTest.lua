---@param array System.Array
function foo(array)
    local leng = array.Length
      for i = 0, leng -1 do
          print(array[i])
      end
      local iter = array:GetEnumerator()
      while iter:MoveNext() do
          print("getEnumerator"..iter.Current)
      end
      local t = array:ToTable()
      for i = 1, #t do
          print("table: "..tostring(t[i]))
      end
     local index = array:BinarySearch(7)
    print('find number index: ',index)
      return leng, '123', true
end
