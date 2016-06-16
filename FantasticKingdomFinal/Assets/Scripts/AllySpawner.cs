using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AllySpawner : MonoBehaviour {
    public GameObject PikemanPrefab;
    public float SpawnTime = 5.0f;
    public IList<GameObject> Squad = new List<GameObject>();
    public int PopulationCap = 3;
    public LayerMask EnemiesLayer = 1 << 8;
    public float LocalTargetBreachRadius = 4.0f;
    public float LocalTargetSeekRadius = 6.0f;
    public float GlobalTargetBreachRadius = 20.0f;
    public float GlobalTargetSeekRadius = 30.0f;

    private Vector3 _spawnPoint;
    private GameObject _goldPile;

    // Use this for initialization
    void Start()
    {
        _goldPile = GameObject.Find("GoldPile");
        InvokeRepeating("SpawnUnit", 0.1f, SpawnTime);

        // Get spawn position
        _spawnPoint = transform.position;// + new Vector3(0, 1, 2);
    }

    void Update()
    {
        bool isAnyEnemyInRange = Physics.OverlapSphere(_goldPile.transform.position, GlobalTargetBreachRadius, EnemiesLayer).Length > 0;
        if (!isAnyEnemyInRange)
            return;

        foreach (var soldier in Squad)
        {
            var localTarget = soldier.GetComponent<Ally>().LocalTarget;
            if (localTarget != null)
                continue;

            var globalTarget = soldier.GetComponent<Ally>().GlobalTarget;
            if (globalTarget != null)
                continue;

            var nextTarget = SeekTarget(_goldPile.transform, GlobalTargetBreachRadius, GlobalTargetSeekRadius, EnemiesLayer);
            soldier.GetComponent<Ally>().GlobalTarget = nextTarget;
        }
    }

    void SpawnUnit()
    {
        bool isAnyEnemyInRange = Physics.OverlapSphere(_goldPile.transform.position, GlobalTargetBreachRadius, EnemiesLayer).Length > 0;

        if ((Squad.Count >= PopulationCap) || !isAnyEnemyInRange)
            return;
        GameObject ally = (GameObject)Instantiate(PikemanPrefab, _spawnPoint, this.transform.rotation);
        ally.transform.Translate(new Vector3(0, 0, 4));
        Ally allyScript = ally.GetComponent<Ally>();
        allyScript.goldPile = GameObject.Find("GoldPile");
        allyScript.speed = 5;
        allyScript.EnemiesLayer = EnemiesLayer;
        allyScript.LocalTargetBreachRadius = LocalTargetBreachRadius;
        allyScript.LocalTargetSeekRadius = LocalTargetSeekRadius;
        allyScript.Home = gameObject;
        AllyHealth allyHealthScript = ally.GetComponent<AllyHealth>();
        allyHealthScript.Squad = Squad;
        Squad.Add(ally);
    }

    public static GameObject SeekTarget(Transform seekSphereCentre, float breachRadius, float searchRadius, LayerMask enemiesLayer)
    {
        // No enemy is in range.
        if (Physics.OverlapSphere(seekSphereCentre.position, breachRadius, enemiesLayer).Length < 1)
            return null;

        var allTargets = Physics.OverlapSphere(seekSphereCentre.position, searchRadius, enemiesLayer);
        var targetCount = allTargets.Length;

        var targetIdx = Random.Range(0, targetCount);
        return allTargets[targetIdx].gameObject;

        //var PotentialTargets = allTargets
        //    .Where(t => t.GetComponent<Goblin>().numberOfAttackers < 1)
        //    .Select(t => t.gameObject);

        //var numOfTargetsInRange = PotentialTargets.Count();
        //if (numOfTargetsInRange > 0)
        //{
        //    var targetIndexPicked = Random.Range(0, numOfTargetsInRange);
        //    return PotentialTargets.ToArray()[targetIndexPicked];
        //}

        //return null;
    }
}
