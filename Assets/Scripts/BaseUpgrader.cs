using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseUpgrader : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject[] castles;
    public TextMeshProUGUI upgradePriceUI;
    public int upgradePriceStep;

    private void Start()
    {
        gameManager.currentCastle = castles[0];
    }

    public void upgradeBase()
    {
        int upgradePrice = int.Parse(upgradePriceUI.text);

        if (gameManager.getCurrentMoney() >= upgradePrice)
        {
            GameObject currentCastle = null;
            GameObject nextCastle = null;
            for (int i = 0; i < castles.Length; i++)
            {
                if (castles[i].activeSelf)
                {
                    currentCastle = castles[i];

                    if (i + 1 < castles.Length)
                    {
                        nextCastle = castles[i + 1];
                        break;
                    }
                }
            }

            if (currentCastle != null && nextCastle != null)
            {
                gameManager.updateMoney(-upgradePrice, true);
                upgradePriceUI.text = (upgradePrice + upgradePriceStep).ToString();
                gameManager.RemoveDefense(currentCastle);
                currentCastle.SetActive(false);
                nextCastle.SetActive(true);
                nextCastle.transform.position = currentCastle.transform.position;
                nextCastle.transform.rotation = currentCastle.transform.rotation;
                nextCastle.transform.SetParent(currentCastle.transform.parent);
                gameManager.currentCastle = nextCastle;
            }
            else
            {
                Debug.LogWarning("AFSDEBUGGING: No castle to upgrade to, or all castles are upgraded.");
            }
        }
        else
        {
            Debug.Log("AFSDEBUGGING: Not enough money!");
            gameManager.uIController.notEnoughMoney();
        }
    }
}
