using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScalarGrid : ExtBehaviour {
	const float foodYPos = 0.15f;
	const float minFoodZPos = -4f;
	const float maxFoodZPos = 4f;

	public float xSize;
	public float zSize;
	public float gridSize;
	private float spawnDelay;
	private float foodSpawnDelay;
	public GameObject[] spawnPoints = new GameObject[2];
	public float decayRate = 4f; // value per second (default = depletes completely in 1/4th second)

	List<int> activeIndices;
	float[] grid;
	bool[] locked;
	int width;
	int height;
	public bool showGridValues;

	void Awake() {
		width = Mathf.CeilToInt(xSize / gridSize);
		height = Mathf.CeilToInt(zSize / gridSize);
		grid = new float[width * height];
		locked = new bool[grid.Length];

		activeIndices = new List<int>((int)(grid.Length * 0.4f));
		Debug.Log("Grid Size: " + width + "x" + height + " = " + grid.Length);
	}

	void Start() {
//		Debug.Log(SMath.FloorMultiple(10.24f, 0.25f));
//		Debug.Log(ClosestIndex(12.34f, 8.90f));
//		Debug.Log(Pos(2289));
//		Debug.DrawRay(Pos(2289), Vector3.up, Color.blue, 10f);
//		HeatPoint(new Vector3(12.34f, 0f, 8.9f), 0.5f);
//

		spawnDelay = GetResourceMgr().antSpawnDelay;
		foodSpawnDelay = GetResourceMgr().foodSpawnDelay;
	}

	void Update() {
		float amt = Time.deltaTime * decayRate;
		for (int i = 0; i < activeIndices.Count; i++) {
			Sub(activeIndices[i], amt);
		}

		if (showGridValues) {
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					Debug.DrawRay(Pos(x, y), Vector3.up * Get(x, y), Color.green);
				}
			}
		}
	}
	
	// ========================================== SETTERS? ========================================== //
	public void SetupLockedIndices() {
		Ray r;
		RaycastHit hit;
		Collider c = collider;
		Vector3 v;

		int mask = (1 << LayerMask.NameToLayer("Level")) | (1 << LayerMask.NameToLayer("Obstacles"));
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				v = Pos(x, y);
				if (Physics.Raycast(v + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, mask)) {
					if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Level")) {	
					} else {
						int ind = Index(x, y);
						if (hit.collider.GetComponent<NeutralSurface>()) {
							Set(ind, 0f);
						} else {
							Set(ind, 1f);
						}
						locked[Index(x, y)] = true;
					}
				}
			}
		}
	}

	// ========================================== SETTERS? ========================================== //
	public void HeatPoint(Vector3 point, float radius, float intensity = 1f) {
		int topLeftIndex = ClosestIndex(point.x - radius, point.z - radius);
		int botRightIndex = ClosestIndex(point.x + radius, point.z + radius);

		int x1 = IndexX(topLeftIndex);
		int z1 = IndexZ(topLeftIndex);
		int x2 = IndexX(botRightIndex);
		int z2 = IndexZ(botRightIndex);

		for (int x = x1; x <= x2; x++) {
			for (int z = z1; z <= z2; z++) {
				Add(x, z, intensity);
			}
		}
	}

	// ========================================== HELPERS ========================================== //
	public float Add(int x, int z, float val) {
		return Add(z * width + x, val);
	}

	public float Add(int i, float val) {
		if (locked[i]) {
			return -1f;
		}
		if (IOOB(i)) {
			return -1f;
		}
		activeIndices.Add(i);
		return grid[i] = Mathf.Clamp01(grid[i] + val);
	}

	public float Sub(int i, float val) {
		if (locked[i]) {
			return -1f;
		}

		if (IOOB(i)) {
			return -1f;
		}

		grid[i] = Mathf.Clamp01(grid[i] - val);
		if (grid[i] == 0) {
			activeIndices.Remove(i);
		}
		return grid[i];
	}
	
	public float Set(int x, int z, float val) {
		return Set(z * width + x, val);
	}

	public float Set(int i, float val) {
		if (locked[i]) {
			return val;
		}
		return grid[i] = Mathf.Clamp01(val);
	}
    
	public int ClosestIndex(float x, float z) {
		return Mathf.RoundToInt((SMath.FloorMultiple(z, gridSize) / gridSize) * width + (SMath.FloorMultiple(x, gridSize) / gridSize));
	}

	public Vector3[] SquaresInBox(Vector3 point, float halfSize) {
		List<Vector3> vectors = new List<Vector3>();
		int topLeftIndex = ClosestIndex(point.x - halfSize, point.z - halfSize);
		int botRightIndex = ClosestIndex(point.x + halfSize, point.z + halfSize);
		
		int x1 = IndexX(topLeftIndex);
		int z1 = IndexZ(topLeftIndex);
		int x2 = IndexX(botRightIndex);
		int z2 = IndexZ(botRightIndex);
		
		for (int x = x1; x <= x2; x++) {
			for (int z = z1; z <= z2; z++) {
				vectors.Add(Pos(x, z));
			}
		}
		return vectors.ToArray();
	}

	public float Get(int x, int z) {
		return Get(z * width + x);
	}

	public float Get(int i) {
		if (IOOB(i)) {
			return 0f;
		}
		return grid[i];
	}

	public Vector3 Pos(int x, int z) {
		return new Vector3(x * gridSize, Get(x, z), z * gridSize);
	}

	public Vector3 Pos(int i) {
		return new Vector3((i % width) * gridSize, Get(i), (i / width) * gridSize);
	}

	public int IndexX(int i) {
		return i % width;
	}

	public int IndexZ(int i) {
		return i / width;
	}

	public int Index(int x, int z) {
		return z * width + x;
	}

	public bool IOOB(int i) {
		return i < 0 || i >= grid.Length;
	}

	public virtual Ant SpawnAnt() {
		System.Random rnd = new System.Random();
		int i = rnd.Next(0, 2);
            
		GameObject newAnt = Instantiate(GetResourceMgr().goAnt, spawnPoints[i].transform.position, spawnPoints[i].transform.rotation) as GameObject;
		newAnt.transform.parent = gameObject.transform;
		newAnt.transform.localPosition = spawnPoints[i].transform.localPosition;
		newAnt.transform.localScale = new Vector3(newAnt.transform.localScale.x, newAnt.transform.localScale.y, newAnt.transform.localScale.z);

		return newAnt.GetComponent<Ant>();
	}

	public virtual Food SpawnFood() {
		GameObject newFood = (GameObject)Instantiate(GetResourceMgr().goFood);
		newFood.transform.parent = gameObject.transform;
		Vector3 rndPos = new Vector3(Random.Range(-7f, 7f), foodYPos, Random.Range(minFoodZPos, maxFoodZPos));
		newFood.transform.localPosition = rndPos;

		return newFood.GetComponent<Food>();
	}

	public virtual GameObject SpawnObstacle(int i) {
		//check for totalObstacleArea;
		//return null if already greater than or equal to totalObstacleArea
		GameObject newObs = null;
		if (i == 0) {
			newObs = (GameObject)Instantiate(GetResourceMgr().goNeutralSurface);

		} else if (i == 1) {
			newObs = (GameObject)GameMgr.Instantiate(GetResourceMgr().goExtremeSurface);
		}

		if (newObs == null)
			return null;

		//randomize scale
		float scaleCoeffX = Random.Range(0.2f, 0.9f);
		float scaleCoeffZ = Random.Range(0.2f, 0.9f);
		float scaleCoeffArea = (scaleCoeffX * scaleCoeffZ);

		GSGame gsGame = (GSGame)GetGameStateMgr().GetCurrentState();
		if (gsGame == null)
			return null;

		if (gsGame.currentObstacleArea + scaleCoeffArea > GetResourceMgr().totalObstacleArea)
			return null;

		gsGame.currentObstacleArea += scaleCoeffArea;

		newObs.transform.localScale = new Vector3(newObs.transform.localScale.x * scaleCoeffX, newObs.transform.localScale.y * scaleCoeffX, newObs.transform.localScale.z * scaleCoeffZ);

		//randomize rotation
		float deg = Random.Range(0f, 179f);
		Vector3 rot = newObs.transform.rotation.eulerAngles;
		newObs.transform.rotation = Quaternion.Euler(rot.x, deg, rot.z);

		//place new obstacle somewhere
		newObs.transform.parent = gameObject.transform;
        
		Vector3 newPos = new Vector3(Random.Range(-4.8f, 4.8f), 0.35f, Random.Range(-3.4f, 3.4f));
		newObs.transform.localPosition = newPos;

		return newObs;
	}

	private void SetSpecialTempVals() {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				Debug.DrawRay(Pos(x, y), Vector3.up * Get(x, y), Color.green);
			}
		}
	}
}
