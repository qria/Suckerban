using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Direction {
    Up, Down, Left, Right
}

static class DirectionMethods
{
    /* Extension class for Direction */
    // Unfortunately this has to use a getter since there is no extension property in csharp
    // so no direction.keyCode but only direction.GetKeyCode()

    public static Dictionary<Direction, KeyCode> directionToKeyCodeDictionary =
        new Dictionary<Direction, KeyCode>()
        {
            {Direction.Up, KeyCode.UpArrow},
            {Direction.Down, KeyCode.DownArrow},
            {Direction.Right, KeyCode.RightArrow},
            {Direction.Left, KeyCode.LeftArrow}
        };

    public static Dictionary<Direction, Vector3> directionToVectorDictionary =
        new Dictionary<Direction, Vector3>()
        {
            {Direction.Up, Vector3.up},
            {Direction.Down, Vector3.down},
            {Direction.Right, Vector3.right},
            {Direction.Left, Vector3.left}
        };

    public static KeyCode GetKeyCode(this Direction direction){
        return directionToKeyCodeDictionary[direction];
    }

    public static Vector3 GetVector(this Direction direction) {
        return directionToVectorDictionary[direction];
    }
}

public class SuckerbanObject : MonoBehaviour {
    /* Can't use GameObjects directly since they are deeply interconnected with the game
     * So we need another layer to control only our game objects
     */
    protected Transform transform;
    protected LevelManager level;

    public void move(Direction direction) {
        transform.position += direction.GetVector();
    }

    public void push(Direction direction)
    {
        // Difference between push and move is that
        // move just moves it and push propagates it

        SuckerbanObject objInFront = level.GetObjectInPosition(transform.position + direction.GetVector());
        if (objInFront != null) {
            objInFront.push(direction);
        }
        move(direction);

    }
}


public class Player : SuckerbanObject {
    void Awake () {
        transform = GetComponent<Transform>();
        level = Object.FindObjectOfType<LevelManager>();
        level.Add(this);
    }
	
	void Update ()
	{
	    List<Direction> directions = new List<Direction>
	    {
	        Direction.Up,
	        Direction.Down,
	        Direction.Left,
	        Direction.Right
	    };

	    foreach (Direction direction in directions)
	    {
	        if (Input.GetKeyDown(direction.GetKeyCode()))
	        {
	            push(direction);
            }
	    }
	}

}
