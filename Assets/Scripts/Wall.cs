using UnityEngine;

public class Wall : SuckerbanObject{

    protected override void AwakeInitialize() {
        transform = GetComponent<Transform>();
        level = FindObjectOfType<LevelManager>();
        level.Add(this);
        foreach (Transform childWall in transform)
        {
            localPositions.Add(childWall.transform.localPosition.ToIntVector2());
        }
    }
}
