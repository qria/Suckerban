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
        switch (itemType) {
            case ItemTypes.None:
                prefab = Resources.Load("Prefabs/Empty");
                break;
            case ItemTypes.Bomb:
                prefab = Resources.Load("Prefabs/BombItem");
                break;
            case ItemTypes.SpeedUp:
                prefab = Resources.Load("Prefabs/SpeedUpItem");
                break;
            case ItemTypes.SpeedDown:
                prefab = Resources.Load("Prefabs/SpeedDownItem");
                break;
            case ItemTypes.AtomicBomb:
                prefab = Resources.Load("Prefabs/AtomicBombItem");
                break;
        }
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
