using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public WeatherService weatherService;
    public bool WeatherServiceEnabled = false;
    public bool isRaining;
    public bool isNight;
    public bool isHot;

    public UIController uIController;

    private DifficultySettings currentDifficultySettings;
    public List<GameObject> defenses;
    public List<GameObject> monsterPrefabs;
    private float currentMoney = 0;
    
    private int currentRound = 1;

    private int monstersKilled = 0;
    public int monsterPerRound;

    public DifficultyConfig difficultyConfig;

    void Start()
    {
        if (WeatherServiceEnabled)
        {
            StartCoroutine(Init());
        }
        
        currentDifficultySettings = difficultyConfig.GetSettings(PlayerPrefs.GetInt("difficulty", 1));

        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Defenses"))
            {
                defenses.Add(obj);
            }
        }
    }

    public int getNumMonstersRound()
    {
        return monsterPerRound; // TODO: the number of max monsters created in each round should depend on the round number
    }

    public void killMonster()
    {
        monstersKilled++;
        if ( monstersKilled == monsterPerRound)
        {
            roundEnded();
        }
    }

    public void roundEnded()
    {
        Debug.Log("Round ended");
        uIController.roundEnded(() => currentRound++, currentRound);
    }

    IEnumerator Init()
    {
        yield return StartCoroutine(weatherService.Init());
        isRaining = weatherService.IsRaining();
        isNight = weatherService.IsNight();
        isHot = weatherService.IsHot();
    }

    void Update()
    {
        updateMoney(currentDifficultySettings.PassiveMoneyEarned * Time.deltaTime, false);
    }

    public void RemoveDefense(GameObject defense)
    {
        defenses.Remove(defense);
    }

    public DifficultySettings CurrentDifficultySettings
    {
        get { return currentDifficultySettings; }
    }

    public void updateMoney(float coins, bool overrideUpdate)
    {
        this.currentMoney += coins;
        uIController.updateMoneyDisplayed(currentMoney, overrideUpdate);
    }

    public float getCurrentMoney()
    {
        return currentMoney;
    }
}
