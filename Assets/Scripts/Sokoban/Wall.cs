using UnityEngine;

public class Wall : SuckerbanObject{

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);
        foreach (Transform childWall in transform)
        {
            localPositions.Add(childWall.transform.localPosition.ToIntVector2());
        }

        isPushable = true;
    }
}
