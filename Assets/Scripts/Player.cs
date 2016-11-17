﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Mono.Security.Cryptography;

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
    
    // For bomberman
    public int bombCount; // How many bombs I've got
    public float bombFuse; // How fast bomb goes BOOM
    public int bombLength; // How long the tail of bomb is
    public bool isAtomicBomb;

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);
        level.currentPlayer = this;

        collider = GetComponent<BoxCollider2D>();
        pushedKeyQueue = new Queue<KeyCode>();

        isPushable = true;

        bombCount = 0;
        bombFuse = 0.75f;
        bombLength = 3;
        isAtomicBomb = false;

        // Simulate keypress when swiped
        var swipeRecognizer = new TKSwipeRecognizer();
        swipeRecognizer.gestureRecognizedEvent += (r) =>
        {
            if (r.completedSwipeDirection == TKSwipeDirection.Up) {
                pushedKeyQueue.Enqueue(KeyCode.UpArrow);
            }
            if (r.completedSwipeDirection == TKSwipeDirection.Down) {
                pushedKeyQueue.Enqueue(KeyCode.DownArrow);
            }
            if (r.completedSwipeDirection == TKSwipeDirection.Right) {
                pushedKeyQueue.Enqueue(KeyCode.RightArrow);
            }
            if (r.completedSwipeDirection == TKSwipeDirection.Left) {
                pushedKeyQueue.Enqueue(KeyCode.LeftArrow);
            }
        };
        TouchKit.addGestureRecognizer(swipeRecognizer);

        // Place Bomb
        // TODO: change this to just pressing space, not directly placing bomb.
        var tapRecognizer = new TKTapRecognizer();
        tapRecognizer.gestureRecognizedEvent += (r) => {
            placeBomb();
        };
        TouchKit.addGestureRecognizer(tapRecognizer);
    }
	
	protected override void UpdateInput ()
    {
        // Queue up direction keys
	    foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
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

        // Suicide
        if (Input.GetKeyDown(KeyCode.Escape)) {
            level.gameOver();
        }

        // Action Key
        if (Input.GetKeyDown(KeyCode.Space)) {
            placeBomb();
        }
    }

    public void gainItem(ItemTypes itemType) {
        // When eating bomberman items.
        switch (itemType) {
            case ItemTypes.Bomb:
                bombCount += 1;
                break;
            case ItemTypes.SpeedUp:
                moveSpeed *= 2;
                bombFuse /= 2;
                break;
            case ItemTypes.SpeedDown:
                moveSpeed /= 2;
                bombFuse *= 2;
                break;
            case ItemTypes.AtomicBomb:
                isAtomicBomb = true;
                break;
        }
    }

    public void placeBomb() {
        if (bombCount > 0 && !(level.GetObjectInPosition(position) is Bomb)) {
            bombCount -= 1;
            Bomb.Create(position, bombFuse, bombLength);
        }
    }
}
