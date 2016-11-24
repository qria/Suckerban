using UnityEngine;


public class TrapPad : SuckerbanObject
{ 
    private BoxCollider2D collider;

    protected override void AwakeInitialize()
    {
        //level.PlaceOnGrid(this);

        collider = GetComponent<BoxCollider2D>();

        isPushable = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //trapM.TrapOn(1);
            /*
            SuckerbanObject Bar = SuckerbanObject.FindObjectOfType<UITrap>;
            Bar.Drop();
            */
            level.gameOver();
        }
    }
}
/*
public class TrapPad : SuckerbanObject
{
    private BoxCollider2D collider;

    public ItemTypes itemType;

    protected override void AwakeInitialize()
    {
        // level.PlaceOnGrid(this); // Don't place on grid to not interact

        collider = GetComponent<BoxCollider2D>();

    }
    void Update()
    {
        if (this.positions == level.currentPlayer.positions)
        {
            trapM.TrapOn(1);
            level.gameOver();//조건 발동 체크용
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            trapM.TrapOn(1);
            level.gameOver();//조건 발동 체크용
            //level.currentPlayer.gainItem(itemType);
        }
    }
}
*/
