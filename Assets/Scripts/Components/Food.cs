using UnityEngine;
using System.Collections;

public class Food : ExtBehaviour {
    public float timeLimit;
    private bool isOwned;
    private Counter decayTimer = new Counter();

    void Start() {
        decayTimer.SetLimit(timeLimit);
    }

    void Update() {
        if (isOwned)
            return;

        decayTimer.Update(Time.deltaTime);

        if (decayTimer.IsReady())
            Destroy(gameObject);
    }

    public bool IsOwned() {
        return isOwned;
    }

    public void SetIsOwned(bool s) {
        isOwned = s;

        if (s) {
            Collider c = gameObject.GetComponent<Collider>();
            c.enabled = false;
        }
    }
}
