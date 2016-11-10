using UnityEngine;

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
    public static IntVector2 operator *(IntVector2 v, int n) {
        return new IntVector2(v.x * n, v.y * n);
    }
}

public static class IntVector2Extensions {
    public static Vector2 ToVector2(this IntVector2 intVector2) {
        return new Vector2(intVector2.x, intVector2.y);
    }

    public static IntVector2 ToIntVector2(this Vector3 vector) {
        return new IntVector2((int)vector.x, (int)vector.y);
    }
}