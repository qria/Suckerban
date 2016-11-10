using UnityEngine;
using System.Collections;

public class Bomb : SuckerbanObject {
    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);

        isPushable = true; // Why? For the glory of satan of course?
    }
}
