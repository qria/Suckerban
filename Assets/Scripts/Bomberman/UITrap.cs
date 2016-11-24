using UnityEngine;

public class UITrap : SuckerbanObject
{
    private BoxCollider2D collider;

    protected override void AwakeInitialize()
    {
        level.PlaceOnGrid(this);

        collider = GetComponent<BoxCollider2D>();

        isPushable = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bomb"))
        {
            Destroy(transform.gameObject);
        }
        else if (other.CompareTag("Player"))
        {
            level.gameOver();
        }
    }
    void Drop() {
        //낙하 코드. 일단 게임오버 처리
        level.gameOver();
    }
    void Update()
    {
      /*  if (trapM.TrapStat(1) == true)
        {
            //낙하 코드. 일단 게임오버 처리
            level.gameOver();
        }
        */
    }
}
