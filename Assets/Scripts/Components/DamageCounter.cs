using UnityEngine;
using System.Collections;

public class DamageCounter : ExtBehaviour {
	private float timer;
	private const float COOLDOWN = 0.55f;
	public bool isDamage = true;
	// Use this for initialization
	void Start () {
		timer = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(new Vector3(0f,0.001f,0f));
		//math taken from http://answers.unity3d.com/questions/174819/damage-gui-pop-up-on-enemy-hit.html
		if(isDamage) {
			guiText.material.color = new Color(guiText.material.color.r, guiText.material.color.g, guiText.material.color.b, Mathf.Cos((Time.time - timer)*((Mathf.PI/2)/COOLDOWN)));
		} else {
			guiText.material.color = new Color(0f, 205f, 0f, Mathf.Cos((Time.time - timer)*((Mathf.PI/2)/COOLDOWN)));
		}
		Destroy(gameObject, COOLDOWN);
	}

}
