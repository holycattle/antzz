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
	private GameObject 			goPlayer = null;
	private GameObject 			goCamera = null;

	private Queue<Ant>          antList = new Queue<Ant>();
	private Queue<Food>         foodList = new Queue<Food>();

	private Counter             spawnDelay = new Counter();
	private Counter             foodSpawnDelay = new Counter();

	private FSM 				fsm = new FSM();
	private List<ResetInfo> 	resetSet = new List<ResetInfo>();

	public GSGame () : base() {
	}

	public override void Enter() {
		InitFSM();
		AddListeners();

		spawnDelay.SetLimit(GameMgr.Instance.GetResourceMgr().antSpawnDelay);
		foodSpawnDelay.SetLimit(GameMgr.Instance.GetResourceMgr().foodSpawnDelay);
	}
	
	// Update is called once per frame
	public override void Update() {
		base.Update();

		fsm.Update();
	}

	void InitFSM() {
		fsm.AddState("Init", InitEnter, InitUpdate, InitExit, true);
		fsm.AddState("Play", PlayEnter, PlayUpdate, PlayExit);
	}

	void AddListeners() {

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

		if (GetNotifyMgr() != null)
			GetNotifyMgr().PostNotify(NotifyType.GameInitState, this);
	}

	public void InitUpdate() {
		fsm.SetState("Play");
	}

	public void InitExit() {
	}
#endregion

	public void PlayEnter() {
	}

	public void PlayUpdate() {
		SpawnAnt();
		SpawnFood();
	}

	public void PlayExit() {

	}

	private void SpawnAnt() {
		ResourceMgr resourceMgr = GameMgr.Instance.GetResourceMgr();

		if (antList.Count < resourceMgr.maxAnts && spawnDelay.IsReady()) {
			Ant newAnt = GameMgr.Instance.grid.SpawnAnt();
			if (newAnt != null)
				antList.Enqueue(newAnt);

			spawnDelay.Reset();
		}

		spawnDelay.Update(Time.deltaTime);
	}

	private void SpawnFood() {
		ResourceMgr resourceMgr = GameMgr.Instance.GetResourceMgr();

		if (foodList.Count < resourceMgr.maxFood && foodSpawnDelay.IsReady()) {
			Food newFood = GameMgr.Instance.grid.SpawnFood();
			if (newFood != null)
				foodList.Enqueue(newFood);

			foodSpawnDelay.Reset();
		}

		foodSpawnDelay.Update(Time.deltaTime);
	}
}
