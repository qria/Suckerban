using UnityEngine;
using System.Collections.Generic;

public class SuckerbanObject : MonoBehaviour {
    /* Can't use GameObjects directly since they are deeply interconnected with the game
     * So we need another layer to control only our game objects
     */
    protected Transform transform;
    protected LevelManager level;

    public void move(Direction direction) {
        transform.position += direction.GetVector();
    }

    public void push(Direction direction) {
        // Difference between push and move is that
        // move just moves it and push propagates it

        SuckerbanObject objInFront = level.GetObjectInPosition(transform.position + direction.GetVector());
        if (objInFront != null) {
            objInFront.push(direction);
        }
        move(direction);

    }
}
