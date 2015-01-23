using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate bool FSMTransitionDg();

public delegate void FSMStateEventDg();
public delegate void VoidDelegate();

public class FSMStateInfo
{
	public FSMStateEventDg enter;
	public FSMStateEventDg update;
	public FSMStateEventDg exit;
	
	public FSMStateInfo(FSMStateEventDg _enter, FSMStateEventDg _update, FSMStateEventDg _exit)
	{
		enter 	= _enter;
		update 	= _update;
		exit 	= _exit;
	}
	
};

public class FSMTransitionInfo
{
	public string from;
	public string to;
	public FSMTransitionDg transition;
	
	public FSMTransitionInfo(string _from, string _to, FSMTransitionDg _transition)
	{
		from = _from;
		to = _to;
		transition = _transition;
	}
};

public class FSM
{
	Dictionary<string, FSMStateInfo> 		states 		= new Dictionary<string, FSMStateInfo>();
	Dictionary<string, FSMTransitionInfo> 	transitions = new Dictionary<string, FSMTransitionInfo>();
	
	public string prevName { get; private set; }
	public string currName { get; private set; }
	public string nextName { get; private set; }
	
	public FSM()
	{
		prevName = "";
		currName = "";
		nextName = "";
	}
	
	public void AddState(	string name, 
	                     FSMStateEventDg enter, 
	                     FSMStateEventDg update, 
	                     FSMStateEventDg exit, 
	                     bool isDefault = false)
	{
		FSMStateInfo info;
		if (states.TryGetValue(name, out info))
		{
			// already exists, lets just change the contents
			info.enter = enter;
			info.update = update;
			info.exit = exit;
			return;
		}
		else
		{
			states.Add(name, new FSMStateInfo(enter, update, exit));
		}
		
		if (isDefault)
		{
			SetState(name);
		}
	}
	
	public void AddTransition( string from, string to, FSMTransitionDg transition)
	{
		string name = GetTransitionName(from, to);
		
		FSMTransitionInfo info;
		if (transitions.TryGetValue(name, out info))
		{
			// already exists, lets just change the contents
			info.from = from;
			info.to = to;
			info.transition = transition;
			return;
		}
		else
		{
			transitions.Add(name, new FSMTransitionInfo(from, to, transition));
		}
	}
	
	public void SetState(string name, bool immediate = false)
	{
		if (name == "") 		return;
		if (name == currName) 	return;
		
		FSMStateInfo info;
		if (states.TryGetValue(name, out info) == false)
		{
			Debug.LogWarning("FSM::SetState(" + name + ") state does not exist");
			return;
		}
		
		nextName = name;
		
		if (immediate)
		{
			Update();
		}
	}
	
	public void Update()
	{
		FSMStateInfo 	currInfo;
		FSMStateInfo 	nextInfo;
		bool 			currValid = false;
		bool 			nextValid = false;
		
		currValid = states.TryGetValue(currName, out currInfo);
		nextValid = states.TryGetValue(nextName, out nextInfo);
		
		if (currValid && nextValid)
		{
			// change state
			if (currInfo.exit != null)
				currInfo.exit();
			
			prevName = currName;
			currName = nextName;
			nextName = "";
			
			if (nextInfo.enter != null)
				nextInfo.enter();
			
		}
		else if (currValid && !nextValid)
		{
			// no change
			if (currInfo.update != null)
				currInfo.update();
			
			// check if we have transitions that are true
			foreach (FSMTransitionInfo transInfo in transitions.Values)
			{
				if (transInfo.from != currName)
					continue;
				
				if (transInfo.transition == null)
					continue;
				
				if (transInfo.transition() == false)
					continue;
				
				SetState(transInfo.to);
				break;
			}
		}
		else if (!currValid && nextValid)
		{
			prevName = currName;
			currName = nextName;
			nextName = "";
			
			// make next the current state
			if (nextInfo.enter != null)
				nextInfo.enter();
		}
		
	}
	
	string GetTransitionName(string from, string to)
	{
		return from + "-" + to;
	}
	
	public string GetPreviousState()
	{
		return prevName;
	}
	public string GetCurrentState()
	{
		return currName;
	}
	public int GetCurrentStateHash()
	{
		return Util.StringToHash(currName);
	}
	public string GetNextState()
	{
		return nextName;
	}
	
	public bool IsEmpty()
	{
		return (states.Count <= 0);
	}
}

