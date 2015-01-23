using UnityEngine;
using System.Collections;

public class AnimatorHelper : ExtBehaviour {
	public string[] states = new string[1];
	
	Animator animator;
	
	Animator GetAnimator() {
		if (animator == null) {
			animator = gameObject.GetComponent<Animator>();
		}
		return animator;
	}
	
	public bool SetAnimation(string baseName, string powerupName) {
		//Util.Log(this.GetType().Name + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
		if (GetAnimator() == null) return false;
		
		string 	combined 		= baseName + powerupName;
		bool	checkCombined	= false;
		bool 	foundCombined 	= false;
		bool 	foundBase	 	= false;
		
		if (baseName != combined)
			checkCombined = true;
		
		foreach (string s in states) {
			if (checkCombined && s == combined) 	
				foundCombined = true;
			
			if (s == baseName) 	
				foundBase = true;
		}
		
		if (foundCombined == false && foundBase == false)
			return false;
		
		string final = foundCombined ? combined : baseName;
		
		if (IsPlaying(final)) {
			//Util.Log("SetAnimation(" + final + ") to " + name + " already playing returning false");
			return false;
		}
		
		//Util.Log("SetAnimation(" + final + ") to " + name);
		
		animator.Play(final);
		return true;
	}
	
	public bool PowerUpAdded(string powerupName) {
		string anim = GetCurrentAnimation();
		if (anim == "") 
			return false;
		
		return SetAnimation(anim, powerupName);
	}
	
	public bool PowerUpRemoved(string powerupName) {
		if (powerupName == "")
			return false;
		
		string anim = GetCurrentAnimation();
		if (anim == "") 
			return false;
		
		if (anim.IndexOf(powerupName) >= 0) {
			anim = anim.Replace(powerupName, "");
			
			Util.Log("PowerUpRemoved(" + powerupName + ") " + anim);
			return SetAnimation(anim, "");
		}
		
		return false;
	}
	
	public string GetCurrentAnimation() {
		if (GetAnimator() == null) return "";
		
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		
		foreach (string s in states)
		{
			if (info.IsName(s))
				return s;
		}
		
		return "";
	}
	
	
	public bool IsPlaying(string name) {
		if (GetAnimator() == null) return false;
		if (animator.layerCount <= 0) return false;
		
		AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
		
		return info.IsName(name);
	}
	
	public void SetFloat(string name, float value) {}
	
	public void SetBool(string name, float value) {}
	
	public void AnimationDone(string name) {
		//Util.Log(this.GetType().Name + "::" + System.Reflection.MethodBase.GetCurrentMethod().Name);
		
		NotifyMgr notifyMgr = GetNotifyMgr();
		if (notifyMgr == null)
			return;
		
		Hashtable data = new Hashtable();
		data.Add("name", name);
		
		notifyMgr.PostNotify(NotifyType.AnimationDone, this, data);
	}
}