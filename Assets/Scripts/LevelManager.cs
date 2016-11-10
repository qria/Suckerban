using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;

public delegate bool Mission();

public class LevelManager : MonoBehaviour
{
    private List<SuckerbanObject> allObjectsOnGrid = new List<SuckerbanObject>();
    private AudioSource BGM;
    private AudioSource DeathSound;
    public Player currentPlayer;
    public GameObject gameOverScreen;
    public string NextLevelName;

    public List<Mission> missions = new List<Mission>();

    void Awake() {
        AudioSource[] Audios = GetComponents<AudioSource>();
        DeathSound = Audios[0];
        BGM = Audios[1];
        gameOverScreen.SetActive(false);
    }
    
	void Update () {
        // Check if all missions are acomplished
        if (missions.All(mission => mission())) {
            // TODO: Show congratz screen here

            // Go to next level
            LoadNextLevel();
        }

        // Restart current scene
        if (Input.GetKeyDown(KeyCode.R)) {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }

        // Cheat to get to next level
        if (Input.GetKeyDown(KeyCode.F1)) {
            LoadNextLevel();
        }
    }

    public void PlaceOnGrid(SuckerbanObject obj) {
        allObjectsOnGrid.Add(obj);
    }

    public bool RemoveFromGrid(SuckerbanObject obj) {
        return allObjectsOnGrid.Remove(obj);
    }

    public void AddMission(Mission mission) {
        missions.Add(mission);
    }

    public void LoadNextLevel() {
        SceneManager.LoadScene(NextLevelName);
    }

    public SuckerbanObject GetObjectInPosition(IntVector2 position)
    {
        // Return the first object in given position 
        foreach(SuckerbanObject obj in allObjectsOnGrid) {
            if (obj.positions.Contains(position)) {
                return obj;
            }
        }
        return null;
    }

    public void gameOver() {
        BGM.Stop();
        DeathSound.Play();
        gameOverScreen.SetActive(true);
        Destroy(currentPlayer.transform.gameObject);
    }
}
