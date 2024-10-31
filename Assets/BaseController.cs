using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public float health;
    public float maxHealth = 20f;

    public GameObject gameOverScreen;
    private float _burnTimer = 0f;
    private bool _burning = false;


    private void Start()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().addDefense(gameObject);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            DestroyObject();
        }
    }

    public void Burn(float seconds)
    {
        if (_burning)
        {
            StartCoroutine(BurnCoroutine(seconds));
        }
        else
        {
            _burnTimer = Mathf.Max(_burnTimer, seconds);
        }
    }

    private IEnumerator BurnCoroutine(float seconds)
    {
        _burning = true;
        _burnTimer = seconds;

        while (_burnTimer > 0f)
        {
            _burnTimer -= Time.deltaTime;
            yield return null;
        }

        _burning = false;
        _burnTimer = 0f;
    }

    protected void DestroyObject()
    {
        gameOverScreen.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = $"YOU SURVIDED {GameObject.Find("GameManager").GetComponent<GameManager>().currentRound} ROUNDS";
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f; // pause the game
    }
}
