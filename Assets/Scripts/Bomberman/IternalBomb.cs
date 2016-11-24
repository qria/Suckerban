using System;
using UnityEngine;
using Object = UnityEngine.Object;

public class IternalBomb : SuckerbanObject {

    public float fuse; // Time until it goes BOOM
    public float delay; // Time until it goes BOOM
    public int bombLength;
    
    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);

        isPushable = false; // Why? For the glory of satan of course?
    }

    public static Object prefab;
    public static IternalBomb Create(IntVector2 position, float fuse, float delay, int bombLength) {
        prefab = Resources.Load("Prefabs/IternalBomb");
        GameObject newObject = Instantiate(prefab) as GameObject;
        IternalBomb bomb = newObject.GetComponent<IternalBomb>();
        bomb.position = position;
        bomb.transform.position = position.ToVector2();
        bomb.fuse = fuse;
        bomb.delay = delay;
        bomb.bombLength = bombLength;

        return bomb;
    }

    protected override void UpdateLogic() {
        fuse -= Time.deltaTime;
        if (fuse < 0)
        {
            ExplodeNCreate();
        }
    }

    public void ExplodeNCreate() {
        BombFlame.Create(position, 0.5f);
        foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
            for (int l = 1; l < bombLength + 1; l++) {
                IntVector2 positionToFlame = position + direction.GetIntVector2()*l;
                SuckerbanObject obj = level.GetObjectInPosition(positionToFlame);
                if (obj is SteelWall) {
                    break;
                }
                if (obj is BrickWall) {
                    ((BrickWall)obj).Destroy();
                    break;
                }
                level.playBombSound();
                BombFlame.Create(positionToFlame, 0.5f);
            }
        }
        Destroy(transform.gameObject);
        IternalBomb.Create(position, delay, delay, bombLength);

    }
}
