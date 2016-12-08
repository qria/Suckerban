using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEngine.UI;

public delegate bool Mission();

public class LevelManager : MonoBehaviour
{
    private List<SuckerbanObject> allObjectsOnGrid = new List<SuckerbanObject>();
    private AudioSource BGM;
    private AudioSource DeathSound;
    public Player currentPlayer;
    public GameObject gameOverScreen;
    public string NextLevelName;

    public GameObject ActionButton;

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
        gameOverScreen.SetActive(false);

        MoveSound = Audios[2];
        PushSound = Audios[3];
        ItemSound = Audios[4];
        SetBombSound = Audios[5];
        BombSound = Audios[6];

        ActionButton = GameObject.Find("ActionButton");
        ActionButton.transform.position = new Vector2(100, 100); // Position the button
        isActionButtonShown = false;
    }
    
	void Update () {
        // Check if all missions are acomplished
        if (missions.All(mission => mission()) || Input.GetKeyDown(KeyCode.F1)) {
            // TODO: Show congratz screen here

            // Go to next level
            LoadNextLevel();
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
