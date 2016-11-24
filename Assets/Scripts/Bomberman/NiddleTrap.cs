using UnityEngine;

public class NiddleTrap : SuckerbanObject
{
    private BoxCollider2D collider;

    protected override void AwakeInitialize()
    {
        //level.PlaceOnGrid(this);
        foreach (Transform childWall in transform)
        {
            localPositions.Add(childWall.transform.localPosition.ToIntVector2());
        }

        collider = GetComponent<BoxCollider2D>();

        isPushable = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            level.gameOver();
        }
        else if (other.CompareTag("Bomb"))
        {
            Destroy(transform.gameObject);
        }
    }

    public void Destroy()
    {
        // Not to be confused with Destroy(GameObject)
        Destroy(transform.gameObject);
    }
}
