using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class Player : SuckerbanObject
{
    private BoxCollider2D collider;
    void Awake () {
        transform = GetComponent<Transform>();
        level = FindObjectOfType<LevelManager>();
        level.Add(this);
        level.currentPlayer = this;

        collider = GetComponent<BoxCollider2D>();
    }
	
	void Update ()
	{
	    var AllDirections = Enum.GetValues(typeof(Direction));
        foreach (Direction direction in AllDirections)
	    {
	        if (Input.GetKeyDown(direction.GetKeyCode()))
	        {
	            move(direction);
	        }
	    }
	}
}
