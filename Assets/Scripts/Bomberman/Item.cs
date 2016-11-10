using UnityEngine;
using System.Collections;


public class Item : SuckerbanObject {
    private BoxCollider2D collider;

    public ItemTypes itemType;

    protected override void AwakeInitialize() {
        // level.PlaceOnGrid(this); // Don't place on grid to not interact

        collider = GetComponent<BoxCollider2D>();
        
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            level.currentPlayer.gainItem(itemType);
            Destroy(this.transform.gameObject);
        }
    }
}
