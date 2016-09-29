using System;
using UnityEngine;
using System.Collections.Generic;


public enum Direction {
    Up, Down, Left, Right
}

static class DirectionMethods {
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

    public static KeyCode GetKeyCode(this Direction direction) {
        return directionToKeyCodeDictionary[direction];
    }

    public static Vector3 GetVector(this Direction direction) {
        return directionToVectorDictionary[direction];
    }
}
