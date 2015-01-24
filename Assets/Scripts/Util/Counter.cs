using UnityEngine;

public class Counter {
    //TODO: add a callback that automatically gets called when finished

	public float limit 	{ get; set; }
	public float current { get; set; }
	
	public Counter() {
		limit = 0;
		current = 0;
	}
	
	public Counter(float _limit) {
		limit = _limit;
		current = limit;
	}
	
	public void SetLimit(float _limit) {
		limit = _limit;
		Reset();
	}
	
	public void Reset() {
		current = limit;
	}
	
	public void Update(float deltaTime) {
		current = (current < 0.0f) ? 0.0f : current - deltaTime;
	}
	
	public bool IsReady() {
		return (current <= 0.0f);
	}
	
	public void ForceReady() {
		current = 0.0f;
	}
	
	public float GetProgress() {
		return (limit - current) / limit;
	}
	
	public float GetInvProgress() {
		return 1.0f - GetProgress();
	}
}
