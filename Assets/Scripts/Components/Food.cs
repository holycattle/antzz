using UnityEngine;
using System.Collections;

public class Food : ExtBehaviour {
    private bool isOwned;

    public bool IsOwned() {
        return isOwned;
    }

    public void SetIsOwned(bool s) {
        isOwned = s;
    }
}
