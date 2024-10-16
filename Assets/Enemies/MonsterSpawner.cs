using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    public Transform castleCenter;       // The center point (castle)
    public float spawnRadius = 20f;      // The radius of the perimeter circle
    public float spawnRate = 3f;         // How often zombies will spawn (in seconds)
    public int maxEnemies = 10;       // The maximum number of enemies that can be spawned
    public GameManager gameManager;
    public GameObject coinPopupPrefab;
    public Terrain terrain;

    private float nextSpawnTime;
    private int enemyCount = 0;
    private int layerMask;

    private int _nightMonster = 3;
    private int _hotMonster = 4;
    private int _rainMonster = 5;

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
        Vector3 spawnPosition = new Vector3(randomPoint.x, terrain.SampleHeight(new Vector3(randomPoint.x, 0f, randomPoint.y)), randomPoint.y) + castleCenter.position;

        int monsterIndex = -1;

        if (gameManager.isNight && gameManager.isRaining)
        {
            bool chooseNightMonster = Random.Range(0, 2) == 0;

            if (chooseNightMonster)
                monsterIndex = _nightMonster;
            else
                monsterIndex = _rainMonster;

        }
        else if (gameManager.isNight)
        {
            bool chooseNightMonster = Random.Range(0, 2) == 0;

            if (chooseNightMonster)
                monsterIndex = _nightMonster;
            else
                monsterIndex = Random.Range(0, gameManager.monsterPrefabs.Count);

        }
        else if (gameManager.isRaining)
        {
            bool chooseRainingMonster = Random.Range(0, 2) == 0;

            if (chooseRainingMonster)
                monsterIndex = _rainMonster;
            else
                monsterIndex = Random.Range(0, gameManager.monsterPrefabs.Count);
        }
        else
        {
            monsterIndex = Random.Range(0, gameManager.monsterPrefabs.Count);
        }

        GameObject monsterPrefab = gameManager.monsterPrefabs[monsterIndex];

        GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

        if (layerMask != -1) // Ensure the layer exists
        {
            if (monsterIndex != _nightMonster)
                monster.layer = layerMask;
        }
        MonsterController script = monster.GetComponent<MonsterController>();
        script.gameManager = gameManager;
        script.applyDifficultySettings(gameManager.CurrentDifficultySettings.MonsterSettings[monsterPrefab.name]);
        script.coinPopupPrefab = coinPopupPrefab;

        enemyCount++;
    }
}
