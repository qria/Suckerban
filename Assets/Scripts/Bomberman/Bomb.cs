using UnityEngine;
using System.Collections;

public class Bomb : SuckerbanObject {

    public float fuse; // Time until it goes BOOM

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);

        isPushable = true; // Why? For the glory of satan of course?
    }

    public static Object prefab;
    public static Bomb Create(IntVector2 position, float fuse) {
        prefab = Resources.Load("Prefabs/Bomb");
        GameObject newObject = Instantiate(prefab) as GameObject;
        Bomb bomb = newObject.GetComponent<Bomb>();
        bomb.position = position;
        bomb.transform.position = position.ToVector2();
        bomb.fuse = fuse;

        return bomb;
    }

    protected override void UpdateLogic() {
        fuse -= Time.deltaTime;
        if (fuse < 0) {
            Explode();
        }
    }

    public void Explode() {
        Destroy(transform.gameObject);
    }
}
