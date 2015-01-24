using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScalarGrid : MonoBehaviour {

	public float xSize;
	public float zSize;
	public float gridSize;

	public float decayRate = 4f; // value per second (default = depletes completely in 1/4th second)

	List<int> activeIndices;
	float[] grid;
	int width;
	int height;

	public bool showGridValues;

	void Awake() {
		width = Mathf.CeilToInt(xSize / gridSize);
		height = Mathf.CeilToInt(zSize / gridSize);
		grid = new float[width * height];

		activeIndices = new List<int>((int)(grid.Length * 0.4f));
		Debug.Log("Grid Size: " + width + "x" + height + " = " + grid.Length);
	}

	void Start() {
//		Debug.Log(SMath.FloorMultiple(10.24f, 0.25f));
//		Debug.Log(ClosestIndex(12.34f, 8.90f));
//		Debug.Log(Pos(2289));
//		Debug.DrawRay(Pos(2289), Vector3.up, Color.blue, 10f);
//		HeatPoint(new Vector3(12.34f, 0f, 8.9f), 0.5f);
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
		if (i < 0 || i > grid.Length) {
			return -1f;
		}
		activeIndices.Add(i);
		return grid[i] = Mathf.Clamp01(grid[i] + val);
	}

	public float Sub(int i, float val) {
		if (i < 0 || i > grid.Length) {
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
		return grid[i] = Mathf.Clamp01(val);
	}
    
	public int ClosestIndex(float x, float z) {
		return Mathf.RoundToInt((SMath.FloorMultiple(z, gridSize) / gridSize) * width + (SMath.FloorMultiple(x, gridSize) / gridSize));
	}

	public float Get(int x, int z) {
		return grid[z * width + x];
	}

	public Vector3 Pos(int x, int z) {
		return new Vector3(x * gridSize, 0f, z * gridSize);
	}

	public Vector3 Pos(int i) {
		return new Vector3((i % width) * gridSize, 0f, (i / width) * gridSize);
	}

	public int IndexX(int i) {
		return i % width;
	}

	public int IndexZ(int i) {
		return i / width;
	}
}
