using UnityEngine;
using System.Collections;

public class SteelWall : SuckerbanObject {

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);
        foreach (Transform childWall in transform) {
            localPositions.Add(childWall.transform.localPosition.ToIntVector2());
        }

        isPushable = false;
    }
}
