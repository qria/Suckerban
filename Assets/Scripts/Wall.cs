using UnityEngine;
using System.Collections.Generic;

public class Wall : SuckerbanObject{

    void Awake() {
        transform = GetComponent<Transform>();
        level = FindObjectOfType<LevelManager>();
        level.Add(this);
        foreach (Transform childWall in transform)
        {
            localPositions.Add(childWall.transform.localPosition);
        }
    }
}
