using UnityEngine;
using System.Collections;
public enum TrapTypes
{
    Item,
    Zone,
    Object,
}
public class TrapManager : SuckerbanObject
{
    protected bool trap1;
	// Use this for initialization
	void Start () {
        trap1 = false;
	
	}
	
	// Update is called once per frame
	void Update () {
        if (trap1 == true)
            level.gameOver();
	}
    public void TrapOn(int i)
    {
        switch (i)
        {
            case 1:
                level.gameOver();
                //trap1 = true;
                break;
        }

    }
    public bool TrapStat(int i)
    {
        switch (i)
        {
            case 1:
                return trap1;
        }
        return false;
    }
}
