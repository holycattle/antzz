using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct ResetInfo {

	public ResetInfo(Transform _t, Vector3 _position)
	{
		t = _t;
		position = _position;
	}
	public Transform t;
	public Vector3 position;

};

public class GSGame : GameState {

	private GameObject 			goPlayer		= null;
	private GameObject 			goCamera 		= null;
	private FSM 				fsm 			= new FSM();
	private List<ResetInfo> 	resetSet 		= new List<ResetInfo>();

	public GSGame() : base() {}

	public override void Enter ()
	{
		InitFSM();
		AddListeners();
	}
	
	// Update is called once per frame
	public override void Update() {
		base.Update();

		fsm.Update();
	}

	void InitFSM() {
		fsm.AddState("Init", 		InitEnter, 		InitUpdate, 	InitExit, 	true);
	}

	void AddListeners() {

	}

	void AddToReset(GameObject g) {
		resetSet.Add(new ResetInfo(g.transform, g.transform.position));
	}

	void ResetState() {
		Util.Log(this.GetType().Name + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
		
		//reset vars
		
		foreach (ResetInfo info in resetSet)
		{
			if (info.t == null) continue;
			info.t.position = info.position;
		}
		
		foreach (ResetInfo info in resetSet)
		{
			if (info.t == null) continue;
			if (info.t.gameObject == null) continue;
			
			GameObject go = info.t.gameObject;
			
			bool active = go.activeSelf;
			go.SetActive(true);
			go.SendMessage("Init", null, SendMessageOptions.DontRequireReceiver);
			go.SetActive(active);
		}

		//reset camera
	}

#region Init state
	public void InitEnter()
	{	
	}
	public void InitUpdate()
	{
		ResetState();
		//fsm.SetState("Generate");	
		
		if (GetNotifyMgr() != null)
			GetNotifyMgr().PostNotify(NotifyType.GameInitState, this);
	}
	public void InitExit()
	{
	}
#endregion
}
