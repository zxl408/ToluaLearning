﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class TestLuaEnumWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(TestLuaEnum));
		L.RegVar("One", get_One, null);
		L.RegVar("Two", get_Two, null);
		L.RegVar("Three", get_Three, null);
		L.RegVar("Four", get_Four, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
		TypeTraits<TestLuaEnum>.Check = CheckType;
		StackTraits<TestLuaEnum>.Push = Push;
	}

	static void Push(IntPtr L, TestLuaEnum arg)
	{
		ToLua.Push(L, arg);
	}

	static bool CheckType(IntPtr L, int pos)
	{
		return TypeChecker.CheckEnumType(typeof(TestLuaEnum), L, pos);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_One(IntPtr L)
	{
		ToLua.Push(L, TestLuaEnum.One);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Two(IntPtr L)
	{
		ToLua.Push(L, TestLuaEnum.Two);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Three(IntPtr L)
	{
		ToLua.Push(L, TestLuaEnum.Three);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Four(IntPtr L)
	{
		ToLua.Push(L, TestLuaEnum.Four);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		TestLuaEnum o = (TestLuaEnum)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}
