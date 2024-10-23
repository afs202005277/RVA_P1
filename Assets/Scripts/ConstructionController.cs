using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    public GameManager gameManager;
    private bool isTracking = false;
    public GameObject towerPrefab;
    public GameObject wallPrefab;
    private GameObject current;

    public List<Transform> GetDirectChildren(Transform obj)
    {
        List<Transform> children = new List<Transform>();

        foreach (Transform child in obj)
        {
            children.Add(child);
        }

        return children;
    }

    private void Update()
    {
        if (current != null)
        {
            current.transform.position = transform.position;
        }
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
            float towerCost = gameManager.CurrentDifficultySettings.TowerCost;
            if (gameManager.getCurrentMoney() >= towerCost)
            {
                GameObject tower = Instantiate(towerPrefab, transform.position, Quaternion.Euler(0, 0, 0));
                tower.GetComponent<MovableTowerController>().gameManager = gameManager;
                current = tower;
                gameManager.addDefense(gameObject);
                gameManager.updateMoney(-towerCost, true);
                isTracking = false;
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
