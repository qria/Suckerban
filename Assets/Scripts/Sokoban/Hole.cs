using System;
using UnityEngine;

public class Hole : MonoBehaviour { // Not SuckerbanObject... not sure if correct choice

    protected Transform transform;
    protected LevelManager level;
    public IntVector2 position; 
    public Mission fillHoleMission;

    public enum FillerType {
        Spike, YellowBall
    }
    public FillerType fillerType; // Which filler to use with this hole

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
        switch (fillerType) {
            case FillerType.Spike:
                return level.GetObjectInPosition(position) is Spike;
            case FillerType.YellowBall:
                return level.GetObjectInPosition(position) is BombermanBall;
        }
        return false;
    }

}
