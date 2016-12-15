using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TitleButton : MonoBehaviour {

    public string firstLevel = "World0Level1";
    public void startHardGame() {
        SceneManager.LoadScene(firstLevel);
    }
}
