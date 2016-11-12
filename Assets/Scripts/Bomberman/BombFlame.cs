using UnityEngine;
using System.Collections;

public class BombFlame : SuckerbanObject {
    private BoxCollider2D collider;

    public float lifetime; // Time until it's gone

    protected override void AwakeInitialize() {
        // level.PlaceOnGrid(this); // Don't place on grid to not interact

        collider = GetComponent<BoxCollider2D>();
    }

    public static Object prefab;
    public static BombFlame Create(IntVector2 position, float lifetime) {
        prefab = Resources.Load("Prefabs/SingleBombFlame");
        GameObject newObject = Instantiate(prefab) as GameObject;
        BombFlame bombFlame = newObject.GetComponent<BombFlame>();
        bombFlame.position = position;
        bombFlame.transform.position = position.ToVector2();
        bombFlame.lifetime = lifetime;

        return bombFlame;
    }
    protected override void UpdateLogic() {
        lifetime -= Time.deltaTime;
        // Destroy self after some time
        if (lifetime < 0) {
            Destroy(transform.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            level.gameOver();
        }
        if (other.CompareTag("BombermanItem")) {
            Destroy(other.gameObject);
        }
    }
}
