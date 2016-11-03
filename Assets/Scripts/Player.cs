using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class Player : SuckerbanObject
{
    // Components
    private BoxCollider2D collider;

    // For movements
    private bool isMoving = false;
    private float remainingMoveDistance;
    private float moveSpeed = 10f;
    private Direction moveDirection;
    
    // Constants 
    private Array AllDirections = Enum.GetValues(typeof(Direction));

    void Awake () {
        transform = GetComponent<Transform>();
        level = FindObjectOfType<LevelManager>();
        level.Add(this);
        level.currentPlayer = this;

        collider = GetComponent<BoxCollider2D>();
    }
	
	void Update ()
    {
	    if (isMoving)
	    {
	        float moveDistance = moveSpeed * Time.deltaTime; // move distance for current frame
            Debug.Log(moveDistance);
	        if (moveDistance >= remainingMoveDistance)
	        {
	            moveDistance = remainingMoveDistance;
	        }
	        move(moveDirection, moveDistance);
	        remainingMoveDistance -= moveDistance;
	        if (remainingMoveDistance < 0.000000001)
	        {
	            isMoving = false;
	        }
	    }
	    else {
            foreach (Direction direction in AllDirections) {
                if (Input.GetKey(direction.GetKeyCode())) // Autofire because delay is there
                {
                    isMoving = true;
                    moveDirection = direction;
                    remainingMoveDistance = 1f;
                }
            }
        }
    }
}
