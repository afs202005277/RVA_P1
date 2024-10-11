using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public List<GameObject> monsterPrefabs; // List of monster prefabs
    public Transform castleCenter;       // The center point (castle)
    public float spawnRadius = 20f;      // The radius of the perimeter circle
    public float spawnRate = 3f;         // How often zombies will spawn (in seconds)
    public int maxEnemies = 10;       // The maximum number of enemies that can be spawned
    public GameManager gameManager;

    private float nextSpawnTime;
    private int enemyCount = 0;
    private int layerMask;

    // Start is called before the first frame update
    void Start()
    {
        nextSpawnTime = Time.time + spawnRate;
        layerMask = LayerMask.NameToLayer("Monsters");
    }

    // Update is called once per frame
    void Update()
    {
        // Check if it's time to spawn a new zombie
        if (Time.time >= nextSpawnTime && enemyCount < maxEnemies)
        {
            SpawnMonster();
            nextSpawnTime = Time.time + spawnRate;  // Set the next spawn time
        }
    }

    void SpawnMonster()
    {
        // Generate a random point on the perimeter of the circle
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPoint.x, 0, randomPoint.y) + castleCenter.position;
        Debug.Log(gameManager.isNight);
        // get random monster prefab
        GameObject monsterPrefab = monsterPrefabs[Random.Range(0, monsterPrefabs.Count)];

        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        if (layerMask != -1) // Ensure the layer exists
        {
            monster.layer = layerMask;
        }
        monster.GetComponent<MonsterController>().gameManager = gameManager;

        enemyCount++;
    }
}
