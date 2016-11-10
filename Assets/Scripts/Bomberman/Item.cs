using UnityEngine;

public class Item : SuckerbanObject {
    private BoxCollider2D collider;

    public ItemTypes itemType;

    protected override void AwakeInitialize() {
        // level.PlaceOnGrid(this); // Don't place on grid to not interact

        collider = GetComponent<BoxCollider2D>();
        
    }

    public static Object prefab;
    public static Item Create(IntVector2 position, ItemTypes itemType) {
        prefab = Resources.Load("Prefabs/BombItem");
        GameObject newObject = Instantiate(prefab) as GameObject;
        Item item = newObject.GetComponent<Item>();
        item.position = position;
        item.transform.position = position.ToVector2();
        item.itemType = itemType;

        return item;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            level.currentPlayer.gainItem(itemType);
            Destroy(this.transform.gameObject);
        }
    }
}
