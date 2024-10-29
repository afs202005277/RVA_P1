using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject towerPrefab;
    public GameObject wallPrefab;
    private GameObject current = null;
    private bool tryingToBuild = false;

    private void Update()
    {
        if (current != null)
        {
            Vector3 currentPos = current.transform.position;
            currentPos.y = gameManager.currentCastle.transform.position.y;
            current.transform.position = currentPos;
        }
    }

    public void onTowerFound()
    {
        if (current == null)
        {
            tryingToBuild = true;
            StartCoroutine(CheckIfCanPlaceDefense(towerPrefab, gameManager.CurrentDifficultySettings.TowerCost));
        }
        else
        {
            current.SetActive(true);
        }
    }

    public void onTargetLost()
    {
        tryingToBuild=false;
        if (current != null)
        {
            current.SetActive(false);
        }
    }

    public void onWallFound()
    {
        if (current == null)
        {
            tryingToBuild = true;
            StartCoroutine(CheckIfCanPlaceDefense(wallPrefab, gameManager.CurrentDifficultySettings.WallCost));
        }
        else
        {
            current.SetActive(true);
        }
    }

    IEnumerator CheckIfCanPlaceDefense(GameObject prefab, float cost)
    {
        while (tryingToBuild)
        {
            if (!gameManager.canPlaceDefense())
            {
                gameManager.uIController.maxDefensesReached();
            }
            else if (gameManager.getCurrentMoney() >= cost)
            {
                GameObject defense = Instantiate(prefab, transform.position, Quaternion.Euler(0, 0, 0));
                defense.transform.SetParent(transform);
                defense.layer = LayerMask.NameToLayer("Defenses");
                defense.GetComponent<DefensiveStructure>().gameManager = gameManager;
                current = defense;
                gameManager.addDefense(defense);
                gameManager.updateMoney(-cost, true);
                tryingToBuild = false;
                yield break;
            }
            else
            {
                gameManager.uIController.notEnoughMoney();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
