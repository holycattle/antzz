using UnityEngine;
using System.Collections;
	
public class ExtBehaviour : MonoBehaviour {
	public NotifyMgr GetNotifyMgr() {
		if (GameMgr.Instance == null) return null;
		return GameMgr.Instance.GetNotifyMgr();
	}
	
	/*public UIMgr GetUIMgr() {
		if (GameMgr.Instance == null) return null;
		return GameMgr.Instance.GetUIMgr();
	}*/
	
	public GameStateMgr GetGameStateMgr() {
		if (GameMgr.Instance == null) return null;
		return GameMgr.Instance.GetGameStateMgr();
	}
	
	/*public UserDataMgr GetUserDataMgr() {
		if (GameMgr.Instance == null) return null;
		return GameMgr.GetUDM();
	}*/
	
	/*public AudioMgr GetAudioMgr() {
		if (GameMgr.Instance == null) return null;
		return GameMgr.Instance.GetAudioMgr();
	}*/
}