using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewGameButton : ExtBehaviour {

	void Start() {
		GetComponent<Button>().onClick.AddListener(OnClick);
		GetNotifyMgr().AddListener(NotifyType.GameTimerUp, OnEndGame);
	}
	
	void OnClick() {
		GetNotifyMgr().PostNotify(NotifyType.NewGame, this);
		gameObject.SetActive(false);
	}

	void OnEndGame(Notify n) {
		gameObject.SetActive(true);
	}
}
