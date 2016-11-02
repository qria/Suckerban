using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class Player : SuckerbanObject {
    void Awake () {
        transform = GetComponent<Transform>();
        level = Object.FindObjectOfType<LevelManager>();
        level.Add(this);
        level.currentPlayer = this;
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

    public override void push(Direction direction, List<SuckerbanObject> alreadyPushedObjects = null) {

        alreadyPushedObjects = alreadyPushedObjects ?? new List<SuckerbanObject>(); // Dynamic parameter 
        // Use alreadyPushedObjects to circumvent recursions
        if (alreadyPushedObjects.Contains(this)) {
            return;
        }
        alreadyPushedObjects.Add(this);

        SuckerbanObject objInFront = level.GetObjectInPosition(transform.position + direction.GetVector());
        if (objInFront != null && objInFront != this)
        {
            if (direction == Direction.Down && objInFront is Triangle)
            {
                level.gameOver();
                return;
            }
            objInFront.push(direction, alreadyPushedObjects);
        }
        move(direction);
    }
}
