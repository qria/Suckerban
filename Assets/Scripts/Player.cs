using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public enum ItemTypes {
    Bomb,
    AtomicBomb,
    SpeedUp,
    SpeedDown
}

public class Player : SuckerbanObject
{
    // Components
    private BoxCollider2D collider;
    private Queue<KeyCode> pushedKeyQueue; // Note: Only deals with direction keys right now

    // Constants 
    private Array AllDirections = Enum.GetValues(typeof(Direction));

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);
        level.currentPlayer = this;

        collider = GetComponent<BoxCollider2D>();
        pushedKeyQueue = new Queue<KeyCode>();

        isPushable = true;
    }
	
	protected override void UpdateInput ()
    {
        // Queue up direction keys
	    foreach (Direction direction in AllDirections) {
	        KeyCode keyCode = direction.GetKeyCode();
            if (Input.GetKeyDown(keyCode)) {
	            pushedKeyQueue.Enqueue(keyCode);
	        }
	    }

        // Process keys when not moving
        if (!isMoving && pushedKeyQueue.Any()) {
	        KeyCode pushedKeyCode = pushedKeyQueue.Dequeue();
	        Direction direction = pushedKeyCode.GetDirection();
            push(direction);
        }

        // Bomb and do stuff
        if (Input.GetKeyDown(KeyCode.Escape)) {
            level.gameOver();
        }
    }

    public void gainItem(ItemTypes itemType) {
        
    }
}
