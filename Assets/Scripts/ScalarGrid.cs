using UnityEngine;
using System.Collections;

public class ScalarGrid : MonoBehaviour {

	public float xSize;
	public float zSize;
	public float gridSize;

	float[] grid;
	int width;
	int height;

	public bool showGridValues;

	void Awake() {
		width = Mathf.CeilToInt(xSize / gridSize);
		height = Mathf.CeilToInt(zSize / gridSize);
		grid = new float[width * height];
		Debug.Log("Grid Size: " + width + "x" + height + " = " + grid.Length);
	}

	void Start() {
//		Debug.Log(SMath.FloorMultiple(10.24f, 0.25f));
		Debug.Log(ClosestIndex(12.34f, 8.90f));
		Debug.Log(Pos(2289));
		Debug.DrawRay(Pos(2289), Vector3.up, Color.blue, 10f);
	}

	void Update() {
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

	}

	// ========================================== HELPERS ========================================== //
	public float Add(int x, int z, int val) {
		return grid[z * width + x] = Mathf.Clamp01(grid[z * width + x] + val);
	}

	public float Set(int x, int z, int val) {
		return grid[z * width + x] = Mathf.Clamp01(val);
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

//	public Vector2 PosXZ(int i) {
//		return new Vector2
//	}
}
