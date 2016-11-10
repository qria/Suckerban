using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class Bomb : SuckerbanObject {

    public float fuse; // Time until it goes BOOM
    public int bombLength;

    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);

        isPushable = true; // Why? For the glory of satan of course?
    }

    public static Object prefab;
    public static Bomb Create(IntVector2 position, float fuse, int bombLength) {
        prefab = Resources.Load("Prefabs/Bomb");
        GameObject newObject = Instantiate(prefab) as GameObject;
        Bomb bomb = newObject.GetComponent<Bomb>();
        bomb.position = position;
        bomb.transform.position = position.ToVector2();
        bomb.fuse = fuse;
        bomb.bombLength = bombLength;

        return bomb;
    }

    protected override void UpdateLogic() {
        fuse -= Time.deltaTime;
        if (fuse < 0) {
            Explode();
        }
    }

    public void Explode() {
        BombFlame.Create(position, 0.5f);
        foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
            for (int l = 1; l < bombLength + 1; l++) {
                IntVector2 positionToFlame = position + direction.GetIntVector2()*l;
                SuckerbanObject obj = level.GetObjectInPosition(positionToFlame);
                if (obj is SteelWall) {
                    break;
                }
                BombFlame.Create(positionToFlame, 0.5f);
            }
        }
        Destroy(transform.gameObject);
    }
}
