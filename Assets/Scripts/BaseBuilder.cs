using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBuilder : MonoBehaviour
{
    public GameManager gameManager;
    public GameObject castlePlaceholder;

    private void Update()
    {
        if (gameManager.currentCastle != null)
        {
            gameManager.currentCastle.transform.position = transform.position;
            gameManager.currentCastle.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void positionCastle()
    {
        castlePlaceholder.transform.position = transform.position;
        gameManager.currentCastle.SetActive(true);
        gameManager.currentCastle.transform.SetParent(castlePlaceholder.transform);
        Debug.Log($"Placed castle on position: {gameManager.currentCastle.transform.position.x}, {gameManager.currentCastle.transform.position.y}, {gameManager.currentCastle.transform.position.z}");
    }

}
