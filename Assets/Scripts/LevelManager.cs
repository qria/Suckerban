using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public delegate bool Mission();

public class LevelManager : MonoBehaviour
{
    private List<SuckerbanObject> allObjects = new List<SuckerbanObject>();
    private AudioSource audio;
    public Player currentPlayer;
    public GameObject gameOverScreen;

    public List<Mission> missions = new List<Mission>();

    void Awake()
    {
        audio = GetComponent<AudioSource>();
        gameOverScreen.SetActive(false);
    }
    
	void Update () {
        // Check if all missions are acomplished
        if (missions.All(mission => mission())) {
            Debug.Log("YOU WON");
        }

        // Restart current scene
        if (Input.GetKeyDown(KeyCode.R)) {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }

    public void Add(SuckerbanObject obj)
    {
        allObjects.Add(obj);
    }

    public void AddMission(Mission mission) {
        missions.Add(mission);
    }

    public SuckerbanObject GetObjectInPosition(IntVector2 position)
    {
        // Return the first object in given position 
        foreach(SuckerbanObject obj in allObjects) {
            if (obj.positions.Contains(position)) {
                return obj;
            }
        }
        return null;
    }

    public void gameOver()
    {
        Debug.Log("YOU DIED");
        audio.Play();
        gameOverScreen.SetActive(true);
        Destroy(currentPlayer.transform.gameObject);
    }
}
