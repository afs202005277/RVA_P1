using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    public GameManager gameManager;
    private bool isTracking = false;

    public List<Transform> GetDirectChildren(Transform obj)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in obj)
        {
            children.Add(child);
        }

        return children;
    }
    public void onTowerFound()
    {
        isTracking = true;
        StartCoroutine(CheckIfCanPlaceTower());
    }

    public void onTowerLost()
    {
        isTracking = false;
    }

    IEnumerator CheckIfCanPlaceTower()
    {
        while (isTracking)
        {
            Transform tower = transform.GetChild(0);
            float towerCost = gameManager.CurrentDifficultySettings.TowerCost;
            if (gameManager.getCurrentMoney() >= towerCost)
            {
                tower.gameObject.SetActive(true);
                gameManager.addDefense(tower.gameObject);
                gameManager.updateMoney(-towerCost, true);
                isTracking = false;
                yield break;
            }
            else
            {
                tower.gameObject.SetActive(false);
                gameManager.uIController.notEnoughMoney();
                Debug.Log("Not enough money to instantiate tower.");
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
