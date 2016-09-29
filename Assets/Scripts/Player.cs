using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class Player : SuckerbanObject {
    void Awake () {
        transform = GetComponent<Transform>();
        level = Object.FindObjectOfType<LevelManager>();
        level.Add(this);
    }
	
	void Update ()
	{
	    var AllDirections = Enum.GetValues(typeof(Direction));
        foreach (Direction direction in AllDirections)
	    {
	        if (Input.GetKeyDown(direction.GetKeyCode()))
	        {
	            push(direction);
	        }
	    }
	}

}
