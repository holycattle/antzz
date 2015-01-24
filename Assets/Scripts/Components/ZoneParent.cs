using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZoneParent : MonoBehaviour {

	public int score = 0;
	public Text scoreText;

	public void IncreaseScore() {
		score++;
		UpdateScoreText();
	}

	void UpdateScoreText() {
		scoreText.text = "" + score + " points";
	}

}
