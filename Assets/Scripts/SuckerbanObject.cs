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

    public void move(Direction direction, float size=1) {
        transform.position += direction.GetVector() * size;
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
        
        if (localPositions.Count <= 1) {
            SuckerbanObject objInFront = level.GetObjectInPosition(transform.position + direction.GetVector());
            if (objInFront != null && objInFront != this)
            {
                objInFront.push(direction, alreadyPushedObjects);
            }
            move(direction);
        }
        else
        {
            // For Walls
            // This algorithm breaks down when multiple surface is juxtaposed
            foreach (Vector2 localPosition in localPositions)
            {
                Vector2 positionInFront = (Vector2) transform.position + localPosition + (Vector2)direction.GetVector();
                SuckerbanObject objInFront = level.GetObjectInPosition(positionInFront);
                if (objInFront != null && objInFront != this) {
                    objInFront.push(direction, alreadyPushedObjects);
                }

            }
            move(direction);
        }

    }
}
