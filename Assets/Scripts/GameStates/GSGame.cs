using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ResetInfo {

	public ResetInfo (Transform _t, Vector3 _position) {
		t = _t;
		position = _position;
	}
	public Transform t;
	public Vector3 position;

};

public class GSGame : GameState {
	public float currentObstacleArea = 0f;

	private float secondTimer;

	private GameObject 			goPlayer = null;
	private GameObject 			goCamera = null;

	private Counter             gameTimer = new Counter();
	private Counter             spawnDelay = new Counter();
	private Counter             foodSpawnDelay = new Counter();

	private FSM 				fsm = new FSM();
	private List<ResetInfo> 	resetSet = new List<ResetInfo>();

	public GSGame () : base() {
	}

	public override void Enter() {
		InitFSM();
		AddListeners();

		gameTimer.SetLimit(GetResourceMgr().gameDuration);
		spawnDelay.SetLimit(GetResourceMgr().antSpawnDelay);
		foodSpawnDelay.SetLimit(GetResourceMgr().foodSpawnDelay);
	}

	// Update is called once per frame
	public override void Update() {
		base.Update();

		fsm.Update();
	}

	void InitFSM() {
		fsm.AddState("Wait", WaitEnter, WaitUpdate, WaitExit, true);
		fsm.AddState("Init", InitEnter, InitUpdate, InitExit);
		fsm.AddState("Play", PlayEnter, PlayUpdate, PlayExit);
		fsm.AddState("End", EndEnter, EndUpdate, EndExit);
	}

	void AddListeners() {
		GetNotifyMgr().AddListener(NotifyType.NewGame, OnNewGame);
	}

	void AddToReset(GameObject g) {
		resetSet.Add(new ResetInfo(g.transform, g.transform.position));
	}

	void ResetState() {
		Util.Log(this.GetType().Name + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
		
		//reset vars
		//reset camera
	}

#region Init state
	public void InitEnter() {
		//RandomSpawn(10);
		gameTimer.Reset();
		spawnDelay.Reset();
		foodSpawnDelay.Reset(); 
        
		currentObstacleArea = 0.0f;
        
		GameMgr.Instance.grid.timer.text = ((int)gameTimer.current).ToString() + " sec.";

		if (GetNotifyMgr() != null)
			GetNotifyMgr().PostNotify(NotifyType.GameInitState, this);
	}

	public void InitUpdate() {
		System.Random rnd = new System.Random();
		GameMgr.Instance.grid.SpawnObstacles();

		GameMgr.Instance.grid.SetupLockedIndices();
		fsm.SetState("Play");
	}

	public void InitExit() {
	}
#endregion

	public void WaitEnter() {
		GetNotifyMgr().PostNotify(NotifyType.GameTimerUp, this);
	}

	public void WaitUpdate() {

	}

	public void WaitExit() {

	}

	public void PlayEnter() {
	}

	public void PlayUpdate() {
		gameTimer.Update(Time.deltaTime);
		secondTimer += Time.deltaTime;
		if (secondTimer >= 1.0f) {
			GameMgr.Instance.grid.timer.text = ((int)gameTimer.current).ToString() + " sec.";
			secondTimer = 0.0f;
		}
		SpawnAnt();
		SpawnFood();

		if (gameTimer.IsReady()) {
			GetNotifyMgr().PostNotify(NotifyType.GameTimerUp, this);
			fsm.SetState("End");
		}
	}

	public void PlayExit() {

	}

	public void EndEnter() {
		Debug.Log("game ended!");

		Ant[] ants = GameMgr.Instance.gameObject.GetComponentsInChildren<Ant>();
		foreach (Ant a in ants)
			GameObject.Destroy(a.gameObject);

		Food[] foodArr = GameMgr.Instance.gameObject.GetComponentsInChildren<Food>();
		foreach (Food f in foodArr)
			GameObject.Destroy(f.gameObject);

		NeutralSurface[] ns = GameMgr.Instance.gameObject.GetComponentsInChildren<NeutralSurface>();
		foreach (NeutralSurface n in ns)
			GameObject.Destroy(n.gameObject);

		ObstacleSurface[] os = GameMgr.Instance.gameObject.GetComponentsInChildren<ObstacleSurface>();
		foreach (ObstacleSurface o in os)
			GameObject.Destroy(o.gameObject);
	}

	public void EndUpdate() {

	}

	public void EndExit() {

	}

	private void SpawnAnt() {
		ResourceMgr resourceMgr = GameMgr.Instance.GetResourceMgr();
		Ant[] antArray = GameMgr.Instance.gameObject.GetComponentsInChildren<Ant>();
		if (antArray.Length < resourceMgr.maxAnts && spawnDelay.IsReady()) {
			GameMgr.Instance.grid.SpawnAnt();
			spawnDelay.Reset();
		} else {
			spawnDelay.Update(Time.deltaTime);
		}
	}

	private void SpawnFood() {
		ResourceMgr resourceMgr = GameMgr.Instance.GetResourceMgr();
		Food[] foodArr = GameMgr.Instance.gameObject.GetComponentsInChildren<Food>();

		if (foodArr.Length < resourceMgr.maxFood && foodSpawnDelay.IsReady()) {
			GameMgr.Instance.grid.SpawnFood();
			foodSpawnDelay.Reset();
		} else {
			foodSpawnDelay.Update(Time.deltaTime);
		}
	}

	private void PopulateNeutralInices() {
	}

	private void PopulateHotIndices() {
	}

	public void OnNewGame(Notify n) {
		Debug.Log("new game!");
		fsm.SetState("Init");
	}
}
