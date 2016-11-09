using System;
using UnityEngine;

public class Hole : MonoBehaviour { // Not SuckerbanObject... not sure if correct choice

    protected Transform transform;
    protected LevelManager level;
    public IntVector2 position; 
    public Mission fillHoleMission;

    void Awake() {
        transform = GetComponent<Transform>();
        fillHoleMission = new Mission(isFilled);
        position = new IntVector2(
            Convert.ToInt32(transform.position.x),
            Convert.ToInt32(transform.position.y)
            );
        level = FindObjectOfType<LevelManager>();

        level.AddMission(fillHoleMission); // Register mission for this level
    }

    public bool isFilled() {
        // Mission for 
        return level.GetObjectInPosition(position) is Spike;
    }

}
