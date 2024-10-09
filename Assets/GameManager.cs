using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<GameObject> defenses;

    // Start is called before the first frame update
    void Start()
    {
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
