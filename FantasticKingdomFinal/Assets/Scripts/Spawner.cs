using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

    private List<Transform> spawnPoints;
    private float spawnTimer;
    private int wave;

    //Table determining the size and composition of enemies in waves
    private int[,] spawns = new int[42, 4] { 
        {10, 1, 0, 0},
        {10, 3, 0 , 0},
        {15, 5, 0, 0 },
        {20, 0, 0 ,1},
        {10, 0, 0 ,3},
        {15, 3, 0 ,3},
        {15, 0, 0 ,0},
		{30, 3, 0 ,5},
        {30, 0, 1 ,0},
        {10, 3, 1 ,5},
		{10, 0, 0 ,0},
        {20, 3, 0 ,5},
        {30, 3, 0, 6 },
        {30, 4, 2 , 7},
        {15, 0, 0 ,0},
        {30, 4, 0 ,7},
		{30, 4, 0 , 7},
        {30, 4, 0 , 8},
        {30, 4, 3 ,8},
		{15, 0, 0, 0 },
        {30, 5, 0 , 8},
		{30, 5, 5 ,10},
		{30, 7, 5 ,10},
		{30, 7, 5 ,10},
		{15, 0, 0 , 0},
		{30, 7, 5 ,10},
		{30, 0, 6, 10 },
		{15, 0, 0 ,15},
		{30, 7, 6 ,15},
		{30, 8, 6, 15 },
		{30, 8, 7, 15 },
		{10, 0, 0 , 0},
		{30, 6, 7, 15 },
		{30, 10, 7 , 15},
		{30, 10, 7 , 15},
        {15, 0, 0 ,0},
		{30, 10, 8 , 20},
		{30, 10, 8 , 20},
		{30, 10, 8 , 20},
		{30, 10, 9 , 20},
        {15, 10, 0 ,0},
		{30, 10, 15 , 30},

    };

    // Use this for initialization
    void Start () {
        spawnPoints = new List<Transform>();
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("SpawnPoint"))
        {
            spawnPoints.Add(g.transform);
        }
        wave = 0;
        spawnTimer = spawns[wave, 0];
    }
	
	// Update is called once per frame
	void Update () {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0)
        {
            Goblin.upStats(1.1f);
            spawn();
            wave++;
            spawnTimer = spawns[wave, 0];
        }
    }

    public int spawnPointNumber()
    {
        return spawnPoints.Count;
    }

    private void spawn()
    {
        int spawnPoint = Random.Range(0, spawnPoints.Count);
        //Spawn Goblins
        for(int i = 0; i < spawns[wave, 1]; i++)
        {
            GameObject newGoblin = (GameObject)Instantiate(Resources.Load("Goblin"), spawnPoints[spawnPoint].position, spawnPoints[spawnPoint].rotation);
            newGoblin.layer = LayerMask.NameToLayer("Enemies");
            newGoblin.gameObject.SetActive(true);
        }
        //Spawn Unicorns
        for (int i = 0; i < spawns[wave, 2]; i++)
        {
            GameObject newUnicorn = (GameObject)Instantiate(Resources.Load("Unicorn"), spawnPoints[spawnPoint].position, spawnPoints[spawnPoint].rotation);
            newUnicorn.layer = LayerMask.NameToLayer("Enemies");
            newUnicorn.gameObject.SetActive(true);
        }

        //Spawn Pigs
        for (int i = 0; i < spawns[wave, 3]; i++)
        {
            GameObject newPig = (GameObject)Instantiate(Resources.Load("Pig"), spawnPoints[spawnPoint].position, spawnPoints[spawnPoint].rotation);
            newPig.layer = LayerMask.NameToLayer("Enemies");
            newPig.gameObject.SetActive(true);
        }

    }
}
