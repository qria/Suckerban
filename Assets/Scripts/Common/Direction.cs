using System;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

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

    public static Dictionary<KeyCode, Direction> KeyCodeToDirectionDictionary =
        new Dictionary<KeyCode, Direction>()
        {
            {KeyCode.UpArrow, Direction.Up},
            {KeyCode.DownArrow, Direction.Down},
            {KeyCode.RightArrow, Direction.Right},
            {KeyCode.LeftArrow, Direction.Left}
        };

    public static Dictionary<Direction, Vector3> directionToVectorDictionary =
        new Dictionary<Direction, Vector3>()
        {
            {Direction.Up, Vector3.up},
            {Direction.Down, Vector3.down},
            {Direction.Right, Vector3.right},
            {Direction.Left, Vector3.left}
        };

    public static Dictionary<Direction, IntVector2> directionToIntVector2Dictionary =
        new Dictionary<Direction, IntVector2>()
        {
            {Direction.Up, IntVector2.up},
            {Direction.Down, IntVector2.down},
            {Direction.Right, IntVector2.right},
            {Direction.Left, IntVector2.left}
        };

    public static Dictionary<TKSwipeDirection, Direction> tkSwipeDirectionToDirectionDictionary =
        new Dictionary<TKSwipeDirection, Direction>()
        {
            {TKSwipeDirection.Up, Direction.Up},
            {TKSwipeDirection.Down, Direction.Down},
            {TKSwipeDirection.Right, Direction.Right},
            {TKSwipeDirection.Left, Direction.Left}
        };

    public static KeyCode GetKeyCode(this Direction direction) {
        return directionToKeyCodeDictionary[direction];
    }

    public static Direction GetDirection(this KeyCode keyCode)
    {
        return KeyCodeToDirectionDictionary[keyCode];
    }

    public static Vector3 GetVector(this Direction direction) {
        return directionToVectorDictionary[direction];
    }

    public static IntVector2 GetIntVector2(this Direction direction) {
        return directionToIntVector2Dictionary[direction];
    }

    public static Direction ToDirection(this TKSwipeDirection tkSwipeDirection) {
        return tkSwipeDirectionToDirectionDictionary[tkSwipeDirection];
    }
    
    public static Array All(this Direction direction){
        return Enum.GetValues(typeof(Direction));
    }
}