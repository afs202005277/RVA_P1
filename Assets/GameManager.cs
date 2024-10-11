using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> defenses;

    // Start is called before the first frame update
    void Start()
    {
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

    void Update()
    {
    }

    public void RemoveDefense(GameObject defense)
    {
        defenses.Remove(defense);
    }
}
