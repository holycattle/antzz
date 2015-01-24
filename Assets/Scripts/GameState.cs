using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//#define DEBUG

public class GameState {
	protected string sceneName = "";
	protected GameObject sceneGameObject = null;
	
	public GameObject goBase { get; set; }
	
	public GameState() {}
	
	public NotifyMgr GetNotifyMgr() {
		return GameMgr.Instance.GetNotifyMgr();
	}

    public ResourceMgr GetResourceMgr() {
        return GameMgr.Instance.GetResourceMgr();
    }

	/*public UIMgr GetUIMgr() {
		return GameMgr.Instance.GetUIMgr();
	}*/
	
	public GameStateMgr GetGameStateMgr() {
		return GameMgr.Instance.GetGameStateMgr();
	}
	
	/*public UserDataMgr GetUserDataMgr() {
		if (GameMgr.Instance == null) return null;
		return GameMgr.GetUDM();
	}*/
	
	public void SwitchToGameState(string name) {
		if (GetGameStateMgr() != null)
			GetGameStateMgr().SetState(name);
	}
	
	public virtual void Enter()	{}
	
	public virtual void Update() {}
	
	public virtual void Exit() {
		if (sceneGameObject)
			Object.Destroy(sceneGameObject);
	}
	
	public virtual GameObject GetPlayerGO() {
		return null;
	}
	
	/// Root gameobject of this state
	public virtual GameObject GetRootGO() {
		return null;
	}
	
	public virtual bool IsPlaying() {
		return true;
	}
	
	public virtual void OnGameDisabled(Notify n) {}
}
