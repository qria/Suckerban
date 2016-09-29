using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    private List<SuckerbanObject> allObjects = new List<SuckerbanObject>();
    private AudioSource audio;
    public Player currentPlayer;

    void Awake()
    {
        audio = GetComponent<AudioSource>();
    }
    
	void Update () {
	    // Check goal here sometime later
	}

    public void Add(SuckerbanObject obj)
    {
        allObjects.Add(obj);
    }

    public SuckerbanObject GetObjectInPosition(Vector2 position)
    {
        // Return the first object in given position 
        foreach(SuckerbanObject obj in allObjects)
        {
            if (obj.localPositions.Count <= 1)
            {
                if ((Vector2) obj.transform.position == position)
                {
                    return obj;
                }
            }
            else
            {
                foreach (Vector2 localPosition in obj.localPositions)
                {
                    if ((Vector2) obj.transform.position + localPosition == position)
                    {
                        return obj;
                    }
                }
            }
           
        }
        return null;
    }

    public void gameOver()
    {
        Debug.Log("YOU DIED");
        audio.Play();
        Destroy(currentPlayer.transform.gameObject);
    }
}
