// UI for items that needs counting
// Shows icon & numbers at the top right corner of the screen.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour {
    private Image BombUIImage;
    private Text BombUIText;
    private LevelManager currentLevel;
    
	void Awake () {
	    BombUIImage = transform.Find("BombUIImage").GetComponent<Image>();
	    BombUIText = transform.Find("BombUIText").GetComponent<Text>();
	    currentLevel = FindObjectOfType<LevelManager>();

	    disableUI();
	}

    void Update() {
        BombUIText.text = currentLevel.currentPlayer.bombCount.ToString();
    }

    public void enableUI() {
        BombUIImage.enabled = true;
        BombUIText.enabled = true;
    }

    public void disableUI() {
        BombUIImage.enabled = false;
        BombUIText.enabled = false;
    }
}
