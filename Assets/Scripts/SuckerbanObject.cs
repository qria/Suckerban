using UnityEngine;
using System.Collections.Generic;

public class SuckerbanObject : MonoBehaviour {
    /* Can't use GameObjects directly since they are deeply interconnected with the game
     * So we need another layer to control only our game objects
     */
    protected Transform transform;
    protected LevelManager level;
    public List<Vector2> localPositions; // If this object is spanned
    // Note that this is relative position to avoid uncessary updates
    public List<Vector2> positions {
        get {
            if (localPositions.Count <= 1) {
                return new List<Vector2>() {transform.position};
            }
            // For Walls
            // This algorithm breaks down when multiple surface is juxtaposed
            return localPositions.ConvertAll(x => (Vector2) transform.position + x);
        }
    }
    
    // For movements
    protected bool isMoving = false;
    protected float remainingMoveDistance;
    public float moveSpeed = 6f;
    protected Direction moveDirection;
    
    void Update() {
        // Broke down Update() into multiple methods
        // It is encouraged not to use  Update()` in children class
        UpdateMove();
        UpdateInput();
    }

    protected virtual void UpdateMove() {
        // Update function that handles movements

        if (!isMoving) { // Only continue when moving
            return;
        }

        float moveDistance = moveSpeed * Time.deltaTime; // move distance for current frame
        if (moveDistance >= remainingMoveDistance) {
            moveDistance = remainingMoveDistance;
        }
        move(moveDirection, moveDistance);
        remainingMoveDistance -= moveDistance;
        if (remainingMoveDistance < 0.000000001) {
            isMoving = false;
        }
    }

    protected virtual void UpdateInput() {
        // Update function that handles inputs
    }


    protected void startMoving(Direction direction, float distance=1f) {
        isMoving = true;
        remainingMoveDistance = distance;
        moveDirection = direction;
    }

    public void move(Direction direction, float distance=1f) {
        transform.position += direction.GetVector() * distance;
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
        
        
        foreach (Vector2 position in positions)
        {
            Vector2 positionInFront = position + (Vector2)direction.GetVector();
            SuckerbanObject objInFront = level.GetObjectInPosition(positionInFront);
            if (objInFront != null && objInFront != this) {
                if (direction == Direction.Down && (this is Player) && (objInFront is Triangle)) { 
                    // If facing spike can't push
                    continue;
                }
                objInFront.push(direction, alreadyPushedObjects);
            }
        }
        startMoving(direction);

    }
}
