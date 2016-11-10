using UnityEngine;

public class Spike : SuckerbanObject {
    private BoxCollider2D collider;

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);

        collider = GetComponent<BoxCollider2D>();

        isPushable = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            level.gameOver();
        }
    }
}
