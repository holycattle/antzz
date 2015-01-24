using UnityEngine;
using System.Collections;

public class Ant : ExtBehaviour {
    void OnCollisionEnter(Collision c) {
        CollectFood(c.gameObject);
    }

    void CollectFood(GameObject g) {
        Food f = g.GetComponent<Food>();
        if (f == null || f.IsOwned())
            return;

        f.SetIsOwned(true);        
        Vector3 oldPos = gameObject.transform.position;
        g.transform.position = new Vector3(oldPos.x, oldPos.y + 0.25f, oldPos.z);
        g.transform.parent = gameObject.transform;

        Debug.Log("collided!");
    }
}
