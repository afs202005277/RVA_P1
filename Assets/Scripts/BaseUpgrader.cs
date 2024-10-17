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
                    Debug.Log(castles[i].name);
                    currentCastle = castles[i];

                    if (i + 1 < castles.Length)
                    {
                        nextCastle = castles[i + 1];
                        Debug.Log("next castle: " + nextCastle.name);
                        break;
                    }
                }
            }

            if (currentCastle != null && nextCastle != null)
            {
                gameManager.updateMoney(-upgradePrice);
                upgradePriceUI.text = (upgradePrice + upgradePriceStep).ToString();
            }
            else
            {
                Debug.LogWarning("No castle to upgrade to, or all castles are upgraded.");
            }
        }
        else
        {
            Debug.Log("Not enough money!");
            gameManager.notEnoughMoney();
        }
    }
}
