using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public delegate bool Mission();

public class LevelManager : MonoBehaviour
{
    private List<SuckerbanObject> allObjectsOnGrid = new List<SuckerbanObject>();
    private AudioSource BGM;
    private AudioSource DeathSound;
    private Image gameOverImage;
    private Image levelClearImage;
    private GameObject ActionButton;

    [HideInInspector]
    public Player currentPlayer;
    [HideInInspector]
    public ItemUI itemUI;

    public string NextLevelName; // If this isn't given, it tries to guess the name of the next level.
    
    private bool _isActionButtonShown;
    public bool isActionButtonShown {
        get { return _isActionButtonShown; }
        set {
            ActionButton.GetComponent<Image>().enabled = value;
            _isActionButtonShown = value;
        }
    }


    private AudioSource ItemSound;
    private AudioSource MoveSound;
    private AudioSource PushSound;
    private AudioSource SetBombSound;
    private AudioSource BombSound;

    public List<Mission> missions = new List<Mission>();

    void Awake() {
        AudioSource[] Audios = GetComponents<AudioSource>();
        DeathSound = Audios[0];
        BGM = Audios[1];
        MoveSound = Audios[2];
        PushSound = Audios[3];
        ItemSound = Audios[4];
        SetBombSound = Audios[5];
        BombSound = Audios[6];

        gameOverImage = GameObject.Find("GameOverImage").GetComponent<Image>();
        gameOverImage.enabled = false;

        levelClearImage = GameObject.Find("LevelClearImage").GetComponent<Image>();
        levelClearImage.enabled = false;
        
        ActionButton = GameObject.Find("ActionButton");
        ActionButton.transform.position = new Vector2(100, 100); // Position the button
        isActionButtonShown = false;


        itemUI = FindObjectOfType<ItemUI>();
    }
    
	void Update () {
        // Check if all missions are acomplished
        if (missions.All(mission => mission())) {
            
            Destroy(currentPlayer.transform.gameObject);

            levelClearImage.enabled = true;

            // Press any key to go to next level
            var recognizer = new TKTapRecognizer();
            recognizer.gestureRecognizedEvent += (r) => {
                LoadNextLevel();
            };
            TouchKit.addGestureRecognizer(recognizer);
        }

        // Restart current scene
        if (Input.GetKeyDown(KeyCode.R)) {
            RestartLevel();
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
    
    public void RestartLevel() {
        TouchKit.removeAllGestureRecognizers(); // Remove all recognizers
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadNextLevel() {
        // Tries to load levels by following order
        // 1. given NextLevelName 
        // 2. current world, level + 1 
        // 3. current world + 1, level 1

        if (NextLevelName != "") { 
            SceneManager.LoadScene(NextLevelName);
        }
        // Try to guess the name of next level if not given
        Scene scene = SceneManager.GetActiveScene();

        Regex regex = new Regex(@"(.*)(\d+)");
        Match match = regex.Match(scene.name);
        if (match.Success) {
            int nextLevel = Int32.Parse(match.Groups[2].Value) + 1;
            string nextLevelName = match.Groups[1].Value + nextLevel;
            if (Application.CanStreamedLevelBeLoaded(nextLevelName)) {
                SceneManager.LoadScene(nextLevelName);
                return;
            }
        }

        regex = new Regex(@"(.*)(\d+)(\w+)(\d+)");
        match = regex.Match(scene.name);
        if (match.Success) {
            int nextWorld = Int32.Parse(match.Groups[2].Value) + 1;
            string nextLevelName = match.Groups[1].Value + nextWorld + match.Groups[3] + "1";
            if (Application.CanStreamedLevelBeLoaded(nextLevelName)) {
                SceneManager.LoadScene(nextLevelName);
                return;
            }
        }

        throw new Exception("Can't find next level!!!");
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
        gameOverImage.enabled = true;
        Destroy(currentPlayer.transform.gameObject);

        // Tap to restart level. Mostly for mobile
        var recognizer = new TKTapRecognizer();
        recognizer.gestureRecognizedEvent += (r) => {
            RestartLevel();
        };
        TouchKit.addGestureRecognizer(recognizer);
    }

    public void playMoveSound()
    {
        MoveSound.Play();
    }
    public void playPushSound()
    {
        PushSound.Play();
    }
    public void playItemSound()
    {
        ItemSound.Play();
    }
    public void playSetBombSound()
    {
        SetBombSound.Play();
    }
    public void playBombSound()
    {
        BombSound.Play();
    }
}
