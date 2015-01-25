using UnityEngine;
using System.Collections;

public class ResourceMgr : ExtBehaviour {
    public GameObject goAnt;
    public GameObject goSoldierAnt;
    public GameObject goFood;
    public GameObject goTermite;
    
    public GameObject goNeutralSurface;
    public GameObject goExtremeSurface;

    public float gameDuration;

    public int maxAnts;
    public float termiteRatio;
    public float soldierAntRatio;
    public float leaderAntRatio;
    public int antGroupCount;

    public int maxFood;

    public float antSpawnDelay;
    public float foodSpawnDelay;

    public float totalObstacleArea;
}
