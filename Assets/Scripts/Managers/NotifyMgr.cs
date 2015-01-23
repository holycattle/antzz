using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Notify
{
	public NotifyType type;
	public object sender;
	public Hashtable data = new Hashtable();
	
	public Notify()
	{
		type = NotifyType.Null;
		sender = null;
	}
	
	public Notify(NotifyType _type, object _sender)
	{
		type = _type;
		sender = _sender;
	}
	public Notify(NotifyType _type, object _sender, Hashtable _data)
	{
		type = _type;
		sender = _sender;
		data = _data;
	}
}

public class NotifyMgr
{
	public delegate void NotifyDelegate(Notify n);
	
	Dictionary<NotifyType, NotifyDelegate> notifyDict = new Dictionary<NotifyType, NotifyDelegate>();
	
	private NotifyDelegate GetNotify(NotifyType type)
	{
		NotifyDelegate del = null;
		if (notifyDict.TryGetValue(type, out del))
		{
			//Util.Log("Found " + type);
		}
		else
		{
			//Util.Log("Not Found " + type);
		}
		
		
		return del;
	}
	
	public void AddListener(NotifyType type, NotifyDelegate n)
	{
		NotifyDelegate del = null;
		if (notifyDict.TryGetValue(type, out del))
		{
			//Util.Log("Adding listener to delegate " + type.ToString());
			notifyDict[type] -= n;
			notifyDict[type] += n;
		}
		else
		{
			//Util.Log("Creating new notify, adding listener " + type.ToString());
			del += n;
			notifyDict.Add(type, del);
		}
	}
	
	public void RemoveListener(NotifyType type, NotifyDelegate n)
	{
		NotifyDelegate del = null;
		if (notifyDict.TryGetValue(type, out del))
		{
			notifyDict[type] -= n;
		}
	}
	
	public void PostNotify(NotifyType type, object sender)
	{
		PostNotify(type, sender, new Hashtable());
	}
	
	public void PostNotify(NotifyType type, object sender, Hashtable data)
	{
		Notify n = new Notify(type, sender, data);
		
		NotifyDelegate del = null;
		if (notifyDict.TryGetValue(type, out del))
			if (del != null)
				del(n);		
	}
}

