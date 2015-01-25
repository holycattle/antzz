using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ZoneParent : ExtBehaviour {

	public int score = 0;
	public Text scoreText;

    void Start() {
        GetNotifyMgr().AddListener(NotifyType.NewGame, OnNewGame);
    }

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

    public void OnNewGame(Notify n) {
        score = 0;
        UpdateScoreText();
    }
}
