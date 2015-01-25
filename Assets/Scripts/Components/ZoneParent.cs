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
        string p = "point";
        if (score == 0 || score > 1)
            p += "s";
		scoreText.text = "" + score + " " + p;
	}

    public void OnNewGame(Notify n) {
        score = 0;
        UpdateScoreText();
    }
}
