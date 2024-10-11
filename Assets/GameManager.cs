using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WeatherService weatherService;
    public List<GameObject> defenses;
    
    public bool WeatherServiceEnabled = true;
    public bool isRaining;
    public bool isNight;
    // Start is called before the first frame update
    void Start()
    {
        if (WeatherServiceEnabled)
        {
            StartCoroutine(Init());
        }
        
        int currentDifficulty = PlayerPrefs.GetInt("difficulty", 1); // Default to "1" if not set
        Debug.Log("Game Difficulty: " + currentDifficulty);

        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Defenses"))
            {
                defenses.Add(obj);
            }
        }

    }

    IEnumerator Init()
    {
        yield return StartCoroutine(weatherService.Init());
        isRaining = weatherService.IsRaining();
        isNight = weatherService.IsNight();
    }

    void Update()
    {
    }

    public void RemoveDefense(GameObject defense)
    {
        defenses.Remove(defense);
    }
}
