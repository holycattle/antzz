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
    private GameObject[]        spawnPoints = new GameObject[2];
	private FSM 				fsm = new FSM();
	private List<ResetInfo> 	resetSet = new List<ResetInfo>();

	public GSGame () : base() {
	}

	public override void Enter() {
		InitFSM();
		AddListeners();

        spawnPoints[0] = Util.Find(GameMgr.Instance.gameObject, "SpawnPoint0");
        spawnPoints[1] = Util.Find(GameMgr.Instance.gameObject, "SpawnPoint1");
	}
	
	// Update is called once per frame
	public override void Update() {
		base.Update();

		fsm.Update();
	}

	void InitFSM() {
		fsm.AddState("Init", InitEnter, InitUpdate, InitExit, true);
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
        RandomSpawn();

        if (GetNotifyMgr() != null)
			GetNotifyMgr().PostNotify(NotifyType.GameInitState, this);
	}

	public void InitUpdate() {

	}

	public void InitExit() {
	}
#endregion

    public virtual void RandomSpawn() {
        System.Random rnd = new System.Random();
        int index = rnd.Next(0, 2);
        GameObject p = spawnPoints[index];

        //GameObject newAnt = Instantiate(GetResourceMgr().goAnt, p.transform.position, Quaternion.identity);
    }
}
