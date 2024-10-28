using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject towerPrefab;
    public GameObject wallPrefab;
    private GameObject current = null;

    private void Update()
    {
        Debug.Log($"TowerPos:{transform.position.y}");
        if (current != null)
        {
            Vector3 currentPos = current.transform.position;
            currentPos.y = gameManager.currentCastle.transform.position.y;
            current.transform.position = currentPos;
        }
    }

    public void onTowerFound()
    {
        Debug.Log("Found Tower");
        if (current == null)
        {
            StartCoroutine(CheckIfCanPlaceDefense(towerPrefab, gameManager.CurrentDifficultySettings.TowerCost));
        }
        else
        {
            current.SetActive(true);
        }
    }

    public void onTargetLost()
    {
        Debug.Log("Lost Tower");
        if (current != null)
        {
            current.SetActive(false);
        }
    }

    public void onWallFound()
    {
        if (current == null)
        {
            StartCoroutine(CheckIfCanPlaceDefense(wallPrefab, gameManager.CurrentDifficultySettings.WallCost));
        }
        else
        {
            current.SetActive(true);
        }
    }

    IEnumerator CheckIfCanPlaceDefense(GameObject prefab, float cost)
    {
        while (true)
        {
            if (!gameManager.canPlaceDefense())
            {
                gameManager.uIController.maxDefensesReached();
                Debug.Log("AFSDEBUGGING: Maximum number of defenses reached.");
            }
            else if (gameManager.getCurrentMoney() >= cost)
            {
                GameObject defense = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 0));
                defense.transform.SetParent(transform);
                defense.layer = LayerMask.NameToLayer("Defenses");
                defense.GetComponent<TowerController>().gameManager = gameManager;
                current = defense;
                gameManager.addDefense(defense);
                gameManager.updateMoney(-cost, true);
                Debug.Log($"Placed Tower on position: {transform.position.x}, {transform.position.y}, {transform.position.z}. Name: {gameObject.name}");
                yield break;
            }
            else
            {
                gameManager.uIController.notEnoughMoney();
                Debug.Log("AFSDEBUGGING: Not enough money to instantiate tower.");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
