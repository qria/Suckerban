﻿using UnityEngine;
using System.Collections;

public class BombermanBall : SuckerbanObject {
    protected override void AwakeInitialize() {
        level.PlaceOnGrid(this);

        isPushable = true;
    }
}
