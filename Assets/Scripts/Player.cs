using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

public class Player : SuckerbanObject
{
    // Components
    private BoxCollider2D collider;

    // Constants 
    private Array AllDirections = Enum.GetValues(typeof(Direction));

    void Awake () {
        transform = GetComponent<Transform>();
        level = FindObjectOfType<LevelManager>();
        level.Add(this);
        level.currentPlayer = this;

        collider = GetComponent<BoxCollider2D>();
    }
	
	protected override void UpdateInput ()
    {
        foreach (Direction direction in AllDirections) {
            if (Input.GetKeyDown(direction.GetKeyCode()) && !isMoving) // Autofire because delay is there
            {
                push(direction);
            }
        }

    }
}
