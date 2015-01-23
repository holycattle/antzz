using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Util
{
	public static bool logsEnabled = true;
	public static event EventHandler _show;

	static public void Log(string message)
	{
		if (logsEnabled == false) // || Debug.isDebugBuild == false)
			return;
		
		Debug.Log(message);
	}
	
	static public void LogWarning(string message)
	{
		Debug.LogWarning(message);
	}
	static public void LogError(string message)
	{
		Debug.LogError(message);
	}
	
	static public GameObject Find(GameObject parent, string name)
	{
		return Find(parent, name, true);
	}
	
	static public GameObject Find(GameObject parent, string name, bool includeDisabled)
	{
		if (parent == null) return null;
		
		int childCount = parent.transform.childCount;
		
		// check for all children
		for (int i=0; i<childCount; i++)
		{
			Transform t = parent.transform.GetChild(i);
			if (!t || !t.gameObject) continue;
			if (includeDisabled == false && t.gameObject.activeSelf == false) continue;
			
			if (String.Compare(t.gameObject.name, name, true) == 0)
				return t.gameObject;
		}
		
		// go inside each child
		for (int i=0; i<childCount; i++)
		{
			Transform t = parent.transform.GetChild(i);
			if (!t || !t.gameObject) continue;
			if (includeDisabled == false && t.gameObject.activeSelf == false) continue;
			
			GameObject go = Find(t.gameObject, name, includeDisabled);
			if (go != null)
				return go;
		}
		
		return null;
	}
	
	static public GameObject[] FindObjectsWithSubString(GameObject parent, string pattern)
	{
		if (parent == null) 
			return new GameObject[0];
		
		List<GameObject> list = new List<GameObject>();
		
		foreach (Transform child in parent.transform)
		{
			if (child == null) continue;
			if (child.gameObject == null) continue;
			
			if (child.gameObject.name.IndexOf(pattern) == -1) continue;
			
			list.Add(child.gameObject);
		}
		
		return list.ToArray();
	}
	
	static public bool GetObjectsWithTag(GameObject parent, string tag, List<GameObject> outSet, bool recursive = true)
	{
		if (parent == null) return false;
		
		if (parent.CompareTag(tag))
			outSet.Add(parent);
		
		int childCount = parent.transform.childCount;
		for (int i=0; i<childCount; i++)
		{
			Transform t = parent.transform.GetChild(i);
			if (t == null) continue;
			if (t.gameObject == null) continue;
			
			GetObjectsWithTag(t.gameObject, tag, outSet);
		}
		return false;
	}
	
	/*static public bool SetUILabel(GameObject parent, string name, string text)
	{
		GameObject go = Util.Find(parent, name);
		if (go == null) return false;
		
		UILabel label = go.GetComponent<UILabel>();
		if (label == null) return false;
		
		label.text = text;
		return true;
	}*/
	
	static public void DeleteAllChildren(GameObject parent, string substring)
	{
		if (parent == null) return;
		
		List<GameObject> list = new List<GameObject>();
		
		foreach (Transform t in parent.transform)
		{
			if (t == null || t.gameObject == null) continue;
			
			string name = t.gameObject.name;
			
			if ((substring.Length == 0) || (name.IndexOf(substring) != -1))
			{
				list.Add(t.gameObject);
			}
		}
		
		foreach(GameObject go in list)
		{
			GameObject.Destroy(go);
		}
		
		list.Clear();
	}
	
	static public void DeleteAllChildrenClones(GameObject parent)
	{
		DeleteAllChildren(parent, "(Clone)");
	}
	
	static public void SetSpriteColor(GameObject go, Color c)
	{
		if (!go) return;
		
		SpriteRenderer spr = go.GetComponent<SpriteRenderer>();
		if (spr)
			spr.color = c;
	}
	
	static public Color GetSpriteColor(GameObject go)
	{
		if (!go) return Color.white;
		
		SpriteRenderer spr = go.GetComponent<SpriteRenderer>();
		if (spr)
			return spr.color;
		
		return Color.white;
	}
	
	static public bool SetAnimation(GameObject go, string name, string powerup)
	{
		if (go == null) return false;
		
		AnimatorHelper helper = go.GetComponent<AnimatorHelper>();
		if (helper == null) return false;
		
		return helper.SetAnimation(name, powerup);
	}
	
	static public bool SetAnimation(GameObject go, string name)
	{
		return SetAnimation(go, name, "");
	}
	
	static public bool IsAnimationPlaying(GameObject go, string name)
	{
		if (go == null) return false;
		
		AnimatorHelper helper = go.GetComponent<AnimatorHelper>();
		if (helper == null) return false;
		
		return helper.IsPlaying(name);
	}
	
	static float worldScreenHeight	= -1.0f;
	static float worldScreenWidth	= -1.0f;
	static float halfHeight			= -1.0f;
	static float halfWidth			= -1.0f;
	
	static public bool IsInMainCamera(GameObject go)
	{
		return IsInMainCamera(GetBounds2D(go));
	}
	
	static public bool IsInMainCamera(Bounds b)
	{
		if (Camera.main == null) 			return false;
		if (Camera.main.transform == null) 	return false;
		
		if (worldScreenHeight < 0.0f)
		{
			worldScreenHeight 	= Camera.main.orthographicSize * 2.0f;
			worldScreenWidth 	= worldScreenHeight / Screen.height * Screen.width;
			halfHeight 			= worldScreenHeight * 0.5f;
			halfWidth 			= worldScreenWidth * 0.5f;
		}
		
		Vector3 pos = Camera.main.transform.position;
		
		Bounds 	camBounds = new Bounds(Vector3.zero, Vector3.zero);
		
		camBounds.min = new Vector3(pos.x - halfWidth, pos.y - halfHeight, 0.0f);
		camBounds.max = new Vector3(pos.x + halfWidth, pos.y + halfHeight, 0.0f);
		
		if (camBounds.Contains( new Vector3(b.min.x, b.min.y) ) ||
		    camBounds.Contains( new Vector3(b.min.x, b.max.y) ) ||
		    camBounds.Contains( new Vector3(b.max.x, b.min.y) ) ||
		    camBounds.Contains( new Vector3(b.max.x, b.max.y) ) )
		{
			return true;
		}
		
		return false;
	}
	
	static public Bounds GetBounds2D(GameObject go)
	{
		Bounds b = new Bounds(Vector3.zero, Vector3.zero);
		
		if (go == null) return b;
		
		int i=0;
		
		if (go.renderer != null)
		{
			b = go.renderer.bounds;
			i++;
		}
		
		
		foreach (Transform t in go.transform)
		{
			if (t == null) continue;
			if (t.renderer == null) continue;
			
			if (i==0)
			{
				b = t.renderer.bounds;
				i++;
				continue;
			}
			
			Bounds currBounds = t.renderer.bounds;
			
			Vector3 min = b.min;
			Vector3 max = b.max;
			
			if (currBounds.min.x < min.x) 	min.x = currBounds.min.x;
			if (currBounds.min.y < min.y) 	min.y = currBounds.min.y;
			
			if (currBounds.max.x > max.x) 	max.x = currBounds.max.x;
			if (currBounds.max.y > max.y) 	max.y = currBounds.max.y;
			
			b.min = min;
			b.max = max;
			
			i++;
		}
		
		//Util.Log("Bounds " + go.name + " = " + b.ToString());
		
		return b;
	}
	
	static public int StringToHash(string str)
	{
		// Lets just use what Unity has
		return Animator.StringToHash(str.ToLower());
	}
	
	static public int StringCompare(string a, string b)
	{
		return StringCompare(a,b,true);
	}
	
	static public int StringCompare(string a, string b, bool caseSensitive)
	{
		if (caseSensitive)
		{
			return System.String.Compare(a, b);
		}
		else
		{
			return System.String.Compare(a, b, System.StringComparison.CurrentCultureIgnoreCase);
		}
	}
}