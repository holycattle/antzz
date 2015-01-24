using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

// this is the ONLY singleton. this class instantiates managers
// IMPORTANT: Make this script exectute after Realtime.
// Go to Edit > Project Settings > Script Execution Order
//
public class GameMgr : MonoBehaviour {		
	byte[] buffer;
	
	//--------------------------------------------------------------------------------
	private static GameMgr instance;
	public static GameMgr Instance {
		get { return instance; }
	}
	
	//--------------------------------------------------------------------------------
	private NotifyMgr notifyMgr = new NotifyMgr();
	public NotifyMgr GetNotifyMgr() {
		return notifyMgr;
	}

    private ResourceMgr resourceMgr;
    public ResourceMgr GetResourceMgr() {
        if (resourceMgr == null)
            resourceMgr = gameObject.GetComponent<ResourceMgr>();
        return resourceMgr;
    }

	//--------------------------------------------------------------------------------
	private GameStateMgr gameStateMgr = new GameStateMgr();
	public GameStateMgr GetGameStateMgr() {
		return gameStateMgr;
	}

	//--------------------------------------------------------------------------------
	/*private UIMgr uiMgr = new RRSV.UIMgr();
	public UIMgr GetUIMgr() {
		return uiMgr;
	}*/
	
	//--------------------------------------------------------------------------------
	/*private UserDataMgr userDataMgr = new RRSV.UserDataMgr();
	public static UserDataMgr GetUDM() {
		if (Instance == null) return null;
		return Instance.userDataMgr;
	}*/
	
	//--------------------------------------------------------------------------------
	/*private AudioMgr audioMgr = new RRSV.AudioMgr();
	public AudioMgr GetAudioMgr() {
		return audioMgr;
	}*/
	
	//--------------------------------------------------------------------------------
	/*private GOPoolMgr poolMgr = new GOPoolMgr();
	public GOPoolMgr GetPoolMgr()
	{
		return poolMgr;
	}*/
	
	//--------------------------------------------------------------------------------
	private GameObject 		goDataMgr = null;
	
	private bool			isLoading = true;

	//--------------------------------------------------------------------------------
	// Game Script references
	public ScalarGrid grid { get; internal set; }
	public InputHandler inputter { get; internal set; }
	public HeatParticles particles { get; internal set; }

	//--------------------------------------------------------------------------------
	void Awake() {
		instance = this;

		// Create the major game states
		gameStateMgr.Add("game", new GSGame(), true);

		GetNotifyMgr().AddListener(NotifyType.LoadGameSceneDone, OnLoadGameSceneDone);
	}
	
	//--------------------------------------------------------------------------------
	/// Use this for initialization
	void Start() {
		Application.targetFrameRate = 60;
		
		// Get References of all important objects
		this.grid = GetComponentInChildren<ScalarGrid>();
		this.inputter = GetComponentInChildren<InputHandler>();
		this.particles = GetComponentInChildren<HeatParticles>();

		#if !(UNITY_EDITOR)
		Util.LogWarning("System Memory " + SystemInfo.systemMemorySize);
		Util.LogWarning("GPU Memory " + SystemInfo.graphicsMemorySize);
		
		if ((Screen.width * Screen.height) <= (1280 * 800)) {
			QualitySettings.masterTextureLimit = 1;
			Util.LogWarning("Using half texture size");
		} else {
			QualitySettings.masterTextureLimit = 0;
			Util.LogWarning("Using full texture size");
		}
		#endif
	}
	
	//--------------------------------------------------------------------------------
	/// Update is called once per frame
	void Update() {
		if (gameStateMgr != null)
			gameStateMgr.Update();
	}
	
	//--------------------------------------------------------------------------------
	void OnGUI() {
	}
	
	public void GarbageCollect(float delay = 0.0f) {
		StartCoroutine(GarbageCollectCR(delay));
	}
	
	//--------------------------------------------------------------------------------
	/// Calls the garbage collector
	private IEnumerator GarbageCollectCR(float delay) {
		if (delay > 0.0f)
			yield return new WaitForSeconds(delay);
		
		// do some cleanup after some time interval
		System.GC.Collect();
		yield return Resources.UnloadUnusedAssets();
	}
	
	//--------------------------------------------------------------------------------
	/// Notify handler for load finished.
	void OnLoadGameSceneDone(Notify n) {
		isLoading = false;

		/*if (audioMgr != null)
			audioMgr.Init();*/

		Debug.Log("load finished");

		GetNotifyMgr().RemoveListener(NotifyType.LoadGameSceneDone, OnLoadGameSceneDone);
	}

	public void OnApplicationFocus(bool focus) {
		Debug.Log("OnApplicationFocus(" + focus + ")");
	}
	
	public void OnApplicationPause(bool pauseStatus) {
		Debug.Log("OnApplicationPause(" + pauseStatus + ")");
		
		if (isLoading) 
			return;
		
		if (pauseStatus == true) {	
			//DoLocalNotification();
		} else {
			CancelLocalNotification();
		}
		
		if (GetNotifyMgr() != null)
			GetNotifyMgr().PostNotify(NotifyType.GameDisabled, this);
	}
	
	public void OnApplicationQuit() {
		if (isLoading)
			return;
		
		if (GetNotifyMgr() != null)
			GetNotifyMgr().PostNotify(NotifyType.GameDisabled, this);
	}
	
	private void CancelLocalNotification() {
	}
	
	private void DoLocalNotification() {
	}
}
