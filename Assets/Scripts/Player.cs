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
    private TKSwipeRecognizer swipeRecognizer;
    private bool isBeingSwiped;
    private TKTapRecognizer tapRecognizer;

    // For bomberman
    public int bombCount; // How many bombs I've got
    public float bombFuse; // How fast bomb goes BOOM
    public int bombLength; // How long the tail of bomb is
    public bool isAtomicBomb;

    // Constants? May need to organize it with global constants
    public float swipeRecognizeDistance = 0.1f; // in cm

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);
        level.currentPlayer = this;

        collider = GetComponent<BoxCollider2D>();

        isPushable = true;

        bombCount = 0;
        bombFuse = 0.75f;
        bombLength = 3;
        isAtomicBomb = false;

        // Simulate keypress when swiped
        swipeRecognizer = new TKSwipeRecognizer(swipeRecognizeDistance);
        swipeRecognizer.gestureRecognizedEvent += (r) => {
            // Since we need to do something Every frame, not just the frame this event gets called,
            // The logic needs to be in Update(), not here.
            isBeingSwiped = true; 
        };
        swipeRecognizer.gestureCompleteEvent += (r) => {
            isBeingSwiped = false;
        };
        TouchKit.addGestureRecognizer(swipeRecognizer);
        
        // Place Bomb
        tapRecognizer = new TKTapRecognizer();
        tapRecognizer.gestureRecognizedEvent += (r) => {
            action();
        };
        TouchKit.addGestureRecognizer(tapRecognizer);
    }
	
	protected override void UpdateInput ()
    {
        // Mobile Direction Control
	    if (isBeingSwiped && !isMoving) {
            push(swipeRecognizer.completedSwipeDirection.ToDirection());
        }

        // PC Direction Control
	    if (!isMoving) {
            foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
                KeyCode keyCode = direction.GetKeyCode();
                if (Input.GetKey(keyCode)) {
                    push(keyCode.GetDirection());
                    break; // Only register one direction key per frame.
                }
            }
        }
        
        // Suicide (for PC)
        if (Input.GetKeyDown(KeyCode.Escape)) {
            level.gameOver();
        }

        // Action Key (for PC)
        // Mobile Action key control is registered via tapRecognizer
        if (Input.GetKeyDown(KeyCode.Space)) {
            action();
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

    public void action() {
        // When action key is pressed
        placeBomb();
    }

    public void placeBomb() {
        if (bombCount > 0 && !(level.GetObjectInPosition(position) is Bomb)) {
            bombCount -= 1;
            Bomb.Create(position, bombFuse, bombLength);
        }
    }
}
