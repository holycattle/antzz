using UnityEngine;
using System.Collections;

public class SceneLoader : ExtBehaviour 
{
	public string[] scenes;
	
	private int currentScene = 0;
	private AsyncOperation asyncOp;
	private float totalProgress = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (totalProgress >= 1.0f)
		{
			return;
		}
		
		if (asyncOp != null)
		{
			float eachScene = 1.0f / (float)scenes.Length;
			totalProgress = (eachScene * (currentScene)) + (eachScene * asyncOp.progress);
			
			//Util.Log(totalProgress.ToString());
			
			Hashtable data = new Hashtable();
			data.Add("progress", totalProgress);
			
			GetNotifyMgr().PostNotify(NotifyType.LoadUpdateProgress, this, data);
		}
	}
	
	public void StartLoading()
	{
		StartCoroutine(Load());
	}
	
	IEnumerator Load()
	{
		const float WAIT_SECS = 1.0f/60.0f;
		
		for (int i=0; i<scenes.Length; i++)
		{
			string scene = scenes[i];
			
			Util.Log("Loading " + scene);
			
			asyncOp = Application.LoadLevelAdditiveAsync(scene);
			currentScene = i;
			yield return asyncOp;
		}
		
		totalProgress -= 0.05f;
		
		yield return new WaitForSeconds(WAIT_SECS);
		
		//ActivateGameObjects();
		
		//yield return new WaitForSeconds(0.5f);
		
		ReorganizeGameObjects();
		
		// Marc: we don't need this anymore because we're putting the run data prefabs into the _Main scene
		// #if USE_RUN_RELEASE
		// 
		// 			// Load data objects from resources, because we need them for the world map
		// 			ResourcesLoader rl = gameObject.AddComponent<ResourcesLoader>();
		// 			if (rl != null)
		// 			{
		// 				rl.Load(ResourcesType.RunData);
		// 				while (rl.IsLoadDone() == false)
		// 					yield return new WaitForSeconds(0.1f);
		// 
		// 				GameObject[] loaded = null;
		// 				loaded = rl.GetLoadedObjects();
		// 				foreach(GameObject go in loaded)
		// 					go.SetActive(true);
		// 
		// 				rl.Load(ResourcesType.RobotData);
		// 				while (rl.IsLoadDone() == false)
		// 					yield return new WaitForSeconds(0.1f);
		// 
		// 				loaded = rl.GetLoadedObjects();
		// 				foreach(GameObject go in loaded)
		// 					go.SetActive(true);
		// 			}
		// #endif
		
		GetNotifyMgr().PostNotify(NotifyType.LoadFinished, this);
		
		totalProgress = 1.0f;
		yield return null;
	}
	
	void ActivateGameObjects()
	{
		GameObject goGameMgr = GameObject.Find("GameMgr");
		if (!goGameMgr) 
		{
			Debug.LogWarning("GameMgr gameobject cannot be found");
			return;
		}
		
		GameObject goUIRoot = Util.Find(goGameMgr, "UI Root");
		if (!goUIRoot) 
		{
			Debug.LogWarning("UI Root gameobject cannot be found");
			return;
		}
		
		// lets pull all objects created under GameMgr
		GameObject[] goA = GameObject.FindGameObjectsWithTag("Root");
		foreach (GameObject go in goA)
		{
			if (!go) continue;
			
			int childCount = go.transform.childCount;
			
			Transform[] childA = new Transform[childCount];
			for (int i=0; i<childCount; i++)
			{
				childA[i] = go.transform.GetChild(i);
			}
			
			foreach (Transform t in childA)
			{
				if (t == null) continue;
				if (t.gameObject == null) continue;
				
				t.parent = goGameMgr.transform;
				t.gameObject.SetActive(true);
			}
		}
	}
	
	void ReorganizeGameObjects()
	{
		GameObject goGameMgr = GameObject.Find("GameMgr");
		if (!goGameMgr) 
		{
			Debug.LogWarning("GameMgr gameobject cannot be found");
			return;
		}
		
		GameObject goUIRoot = Util.Find(goGameMgr, "UI Root");
		if (!goUIRoot) 
		{
			Debug.LogWarning("UI Root gameobject cannot be found");
			return;
		}
		
		// lets pull all objects created under GameMgr
		GameObject[] goA = GameObject.FindGameObjectsWithTag("Root");
		foreach (GameObject go in goA)
		{
			if (!go) continue;
			
			foreach (Transform t in go.transform)
			{
				if (t.gameObject.name == "UI Root")
				{
					// the ui root from another scene, lets get all except camera
					// and dump this ui root
					
					/*int childIdx 			= 0;
					int uiChildCount 		= t.gameObject.transform.childCount;
					Transform[] uiChildA 	= new Transform[uiChildCount];
					
					for (int j=0; j<uiChildCount; j++)
					{
						Transform tUIChild = t.gameObject.transform.GetChild(j);
						if (!tUIChild) continue;
						
						GameObject goUIChild = tUIChild.gameObject;
						if (!goUIChild) continue;
						
						UICamera cUICamera = goUIChild.GetComponent<UICamera>();
						if (cUICamera) continue; // we dont want cameras
						
						uiChildA[childIdx++] = tUIChild;
					}
					
					for (int j=0; j<childIdx; j++)
					{
						uiChildA[j].parent = goUIRoot.transform;
					}*/
					
					Destroy(t.gameObject);
					
				}
				else
				{
					t.parent = goGameMgr.transform;
					t.gameObject.SetActive(false);
				}
			}
			
			Destroy(go);
		}
		
		Destroy(this);
	}
}