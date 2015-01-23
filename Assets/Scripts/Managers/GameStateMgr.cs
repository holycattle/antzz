using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameStateMgr {
	FSM fsm = new FSM();
	
	Dictionary<string, GameState> states = new Dictionary<string, GameState>();
	
	public void Add(string name, GameState state, bool isDefault = false) {
		states.Add(name, state);
		fsm.AddState(name, StateEnter, StateUpdate, StateExit, isDefault);
	}
	
	public void SetState(string name) {
		fsm.SetState(name);
	}
	
	public void Update() {
		fsm.Update();
	}
	
	public void StateEnter() {
		GameState gs;
		if (states.TryGetValue(fsm.GetCurrentState(), out gs) == false)
			return;
		
		gs.Enter();
	}
	
	public void StateUpdate() {
		GameState gs;
		if (states.TryGetValue(fsm.GetCurrentState(), out gs) == false)
			return;
		
		gs.Update();
	}
	
	public void StateExit() {
		GameState gs;
		if (states.TryGetValue(fsm.GetCurrentState(), out gs) == false)
			return;
		
		gs.Exit();
	}
	
	public GameState GetGameState(string name) {
		GameState gs;
		if (states.TryGetValue(name, out gs))
			return gs;
		
		return null;
	}
	
	public GameState GetCurrentState() {
		return GetGameState(fsm.GetCurrentState());
	}
	
	public string GetCurrentStateName() {
		return fsm.GetCurrentState();
	}
}
