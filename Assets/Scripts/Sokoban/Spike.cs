using UnityEngine;

public class Spike : SuckerbanObject {
    private BoxCollider2D collider;

    protected override void AwakeInitialize() {
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
