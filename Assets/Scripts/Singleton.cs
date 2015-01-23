using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
	protected static T instance;

	private static bool overrideGrouping;
	private static GameObject containerGameObject;
	
	// Returns the instance of this singleton.
	// This will look for an exisiting instance first,
	// followed by a prefab (inside Resources) with the same name as the class.
	// If no instance or prefab is found, a new instance will be created.
	
	public static T Instance {
		get {
			if(instance == null) {
				instance = (T) FindObjectOfType(typeof(T));
				
				if (instance == null) {

					Object obj = Resources.Load<GameObject>(typeof(T).Name);

					if (obj != null) {
						GameObject go = Instantiate(obj) as GameObject;
						instance = go.GetComponent<T>();
					} else {
						//Debug.LogError("An instance of " + typeof(T) + 
						//               " is needed in the scene, but there is none.");
						GameObject go = new GameObject(typeof(T).Name);
						instance = go.AddComponent<T>();						
					}
				}
			}

			containerGameObject = GameObject.Find("Singletons");
			if (containerGameObject == null) {
				containerGameObject = new GameObject("Singletons");
				DontDestroyOnLoad(containerGameObject);
			}

			if (!overrideGrouping) {
				instance.gameObject.transform.parent = containerGameObject.transform;
			}
			
			return instance;
		}
	}

	public static T Init() {
		return Instance;
	}

	protected void OverrideSingletonGrouping() {
		overrideGrouping = true;
	}
}
