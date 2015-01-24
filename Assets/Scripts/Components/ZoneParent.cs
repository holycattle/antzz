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

    public void DecreaseScore(int penalty) {
        if (score == 0)
            return;

        score -= penalty;
        UpdateScoreText();
    }

	void UpdateScoreText() {
		scoreText.text = "" + score + " points";
	}

}
