using UnityEngine;
using System.Collections.Generic;

public class Triangle : SuckerbanObject {
    private BoxCollider2D collider;

    void Awake() {
        transform = GetComponent<Transform>();
        level = FindObjectOfType<LevelManager>();
        level.Add(this);

        collider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            level.gameOver();
        }
    }
}
