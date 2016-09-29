using UnityEngine;
using System.Collections.Generic;

public class Triangle : SuckerbanObject {
    void Awake() {
        transform = GetComponent<Transform>();
        level = Object.FindObjectOfType<LevelManager>();
        level.Add(this);
    }
}
