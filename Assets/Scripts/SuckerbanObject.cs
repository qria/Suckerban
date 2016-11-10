using System;
using UnityEngine;
using System.Collections.Generic;

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
            if (localPositions.Count <= 0) {
                return new List<IntVector2> {position};
            }
            // For Walls
            // This algorithm breaks down when multiple surface is juxtaposed
            return localPositions.ConvertAll(x => position + x);
        }
    }
    
    public bool isPushable = false; // Set this to true if you want it to be pushable

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

    public virtual void push(Direction direction) {
        // Pushing success! Proceeding to move
        List<SuckerbanObject> alreadyPushedObjects = new List<SuckerbanObject>();
        bool pushSuccess = checkPushable(direction, alreadyPushedObjects);
        if (!pushSuccess) {
            // Push failed. Bail out!
            return;
        }
        foreach (SuckerbanObject obj in alreadyPushedObjects) {
            obj.position += direction.GetIntVector2();
            obj.startMovingAnimation(direction);
        }
    }
    
    public virtual bool checkPushable(Direction direction, List<SuckerbanObject> alreadyPushedObjects) {
        // Check if pushable and calculate what objects should be pushed.
        // returns True if pushable
        
        // To circumvent recursions
        if (alreadyPushedObjects.Contains(this)) {
            return true;
        }
        alreadyPushedObjects.Add(this);

        
        foreach (IntVector2 sub_position in positions)
        {
            IntVector2 positionInFront = sub_position + (IntVector2)direction.GetIntVector2();
            SuckerbanObject objInFront = level.GetObjectInPosition(positionInFront);
            if (objInFront == null) {
                // There's nothing in front
                continue;
            }
            if (!objInFront.isPushable) {
                // Unpushable object is in front
                return false;
            }
            if (objInFront == this) {
                // Ignore if it's me
                continue;
            }
            if (direction == Direction.Down && (this is Player) && (objInFront is Spike)) { 
                // If facing spike can't push
                continue;
            }

            bool pushSuccess = objInFront.checkPushable(direction, alreadyPushedObjects);
            if (!pushSuccess) {
                return false; // Propagate failness
            }
        }
        return true;
    }
}
