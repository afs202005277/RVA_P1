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
        if (current != null)
        {
            current.transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, 0));
        }
    }

    public void onTowerFound()
    {
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
                gameManager.uIController.notEnoughMoney();
                Debug.Log("Maximum number of defenses reached.");
            }
            else if (gameManager.getCurrentMoney() >= cost)
            {
                GameObject defense = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 0));
                defense.GetComponent<TowerController>().gameManager = gameManager;
                current = defense;
                gameManager.addDefense(gameObject);
                gameManager.updateMoney(-cost, true);
                yield break;
            }
            else
            {
                gameManager.uIController.notEnoughMoney();
                Debug.Log("Not enough money to instantiate tower.");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
