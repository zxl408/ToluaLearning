using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassLua : MonoBehaviour {
    string script = @"
          Person={}
          function Person:new(o,arg,name)
          o = o or {}
          setmetatable(o, self)
          self.__index = self
          self.arg = arg or 0
          self.name = name or ''
          
           return o
           end
";
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
