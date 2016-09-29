using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private List<SuckerbanObject> allObjects = new List<SuckerbanObject>();
    
	void Update () {
	
	}

    public void Add(SuckerbanObject obj)
    {
        allObjects.Add(obj);
    }

    public SuckerbanObject GetObjectInPosition(Vector2 position)
    {
        foreach(SuckerbanObject obj in allObjects)
        {
            if ((Vector2) obj.transform.position == position)
            {
                return obj;
            }
        }
        return null;
    }
}
