using UnityEngine;
using System.Collections;

public class BrickWall : SuckerbanObject {

    public ItemTypes itemInside;

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);
        foreach (Transform childWall in transform) {
            localPositions.Add(childWall.transform.localPosition.ToIntVector2());
        }

        isPushable = false;
    }

    public void Destroy() {
        // Not to be confused with Destroy(GameObject)

        Item.Create(position, ItemTypes.Bomb);
        Destroy(transform.gameObject);
    }
}