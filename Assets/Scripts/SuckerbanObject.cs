using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Structs;

public class SuckerbanObject : MonoBehaviour {
    /* Can't use GameObjects directly since they are deeply interconnected with the game
     * So we need another layer to control only our game objects
     * 
     * **IMPORTANT** 
     * Note that `position` and `transform.position` are different.
     * position :-> int
     * transform.position :-> float
     * 
     * `transform.position` gets synchronized with `position` every frame if not moving
     * 
     * TODO: Make clear distinction of `transform.position` and `position`, and functions that use them.
     */
    protected Transform transform;
    protected LevelManager level;
    public IntVector2 position; // DO NOT CONFUSE THIS WITH `transform.position`
    public List<IntVector2> localPositions = new List<IntVector2>(); // If this object is spanned in multiple positions
    // Note that this is relative position to avoid uncessary updates
    public List<IntVector2> positions {
        get {
            if (localPositions.Count <= 1) {
                return new List<IntVector2> {position};
            }
            // For Walls
            // This algorithm breaks down when multiple surface is juxtaposed
            return localPositions.ConvertAll(x => position + x);
        }
    }
    
    // For movements
    protected bool isMoving = false;
    protected float remainingMoveDistance;
    public float moveSpeed = 6f;
    protected Direction moveDirection;

    void Awake() {
        // Override `AwakeInitialize()` to use `Awake()`
        // It is strongly encouraged not to use  `Awake()` in children class
        // There might be a better way to deal with subclassing and Awake.
        
        AwakeInitialize();
        position = new IntVector2(
            Convert.ToInt32(transform.position.x),
            Convert.ToInt32(transform.position.y)
            );
    }

    protected virtual void AwakeInitialize() {
        // Override this to initialize
    }

    void Update() {
        // Broke down Update() into multiple methods
        // It is strongly encouraged not to use `Update()` in children class
        UpdateMoveAnimation();
        UpdateInput();
    }

    protected virtual void UpdateMoveAnimation() {
        // Update function that handles movements

        if (!isMoving) { // Only continue when moving
            transform.position = position.ToVector2();
            return;
        }

        float moveDistance = moveSpeed * Time.deltaTime; // move distance for current frame
        if (moveDistance >= remainingMoveDistance) {
            moveDistance = remainingMoveDistance;
        }
        transform.position += moveDirection.GetVector() * moveDistance;
        remainingMoveDistance -= moveDistance;
        if (remainingMoveDistance < 0.000000001) {
            isMoving = false;
        }
    }

    protected virtual void UpdateInput() {
        // Update function that handles inputs
    }


    protected void startMovingAnimation(Direction direction, float distance=1f) {
        isMoving = true;
        remainingMoveDistance = distance;
        moveDirection = direction;
    }
    
    public virtual void push(Direction direction, List<SuckerbanObject> alreadyPushedObjects=null) {
        // Difference between push and move is that
        // move just moves it and push propagates it

        alreadyPushedObjects = alreadyPushedObjects ?? new List<SuckerbanObject>(); // Dynamic parameter 

        // Use alreadyPushedObjects to circumvent recursions
        if (alreadyPushedObjects.Contains(this)) {
            return;
        }
        alreadyPushedObjects.Add(this);
        
        
        foreach (IntVector2 sub_position in positions)
        {
            IntVector2 positionInFront = sub_position + (IntVector2)direction.GetIntVector2();
            SuckerbanObject objInFront = level.GetObjectInPosition(positionInFront);
            if (objInFront != null && objInFront != this) {
                if (direction == Direction.Down && (this is Player) && (objInFront is Triangle)) { 
                    // If facing spike can't push
                    continue;
                }
                objInFront.push(direction, alreadyPushedObjects);
            }
        }

        // Pushing success! Proceeding to move
        position += direction.GetIntVector2();
        startMovingAnimation(direction);
    }
}
