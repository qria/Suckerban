using System;
using UnityEngine;
using System.Collections.Generic;


public struct IntVector2 {
    public int x, y;

    public IntVector2(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public override string ToString() {
        return string.Format("({0},{1})", x, y);
    }

    public static IntVector2 up = new IntVector2(0, 1);
    public static IntVector2 down = new IntVector2(0, -1);
    public static IntVector2 right = new IntVector2(1, 0);
    public static IntVector2 left = new IntVector2(-1, 0);

    public static IntVector2 operator +(IntVector2 v1, IntVector2 v2) {
        return new IntVector2(v1.x + v2.x, v1.y + v2.y);
    }
}

public static class IntVector2Extensions
{
    public static Vector2 ToVector2(this IntVector2 intVector2)
    {
        return new Vector2(intVector2.x, intVector2.y);
    }

    public static IntVector2 ToIntVector2(this Vector3 vector)
    {
        return new IntVector2((int)vector.x, (int)vector.y);
    }
}

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
        
        
        foreach (IntVector2 _position in positions)
        {
            IntVector2 positionInFront = _position + (IntVector2)direction.GetIntVector2();
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
