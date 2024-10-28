using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public WeatherService weatherService;
    public bool WeatherServiceEnabled = false;
    public bool isRaining;
    public bool isNight;
    public bool isHot;

    public Light skylight;
    public Color dayColor;
    public Color nightColor;

    public ParticleSystem rainParticles;

    public UIController uIController;

    private DifficultySettings currentDifficultySettings;
    public List<GameObject> defenses;
    public List<GameObject> monsterPrefabs;
    private float currentMoney = 0;
    
    public int currentRound = 1;
    private bool waitingForRound = false;

    private int monstersKilled = 0;
    public int monsterPerRound;

    public DifficultyConfig difficultyConfig;

    public int maxDefenses;

    public GameObject currentCastle;

    void Start()
    {
        Time.timeScale = 1f;
        if (WeatherServiceEnabled)
        {
            StartCoroutine(Init());
        }

        setGameWeather();

        currentDifficultySettings = difficultyConfig.GetSettings(PlayerPrefs.GetInt("difficulty", 1));
    }

    public void setGameWeather()
    {
        skylight.color = isNight ? nightColor : dayColor;
        if (isRaining) rainParticles.Play();
    }

    public int getNumMonstersRound()
    {
        return monsterPerRound + (currentRound-1); // TODO: the number of max monsters created in each round should depend on the round number
    }

    public void killMonster()
    {
        monstersKilled++;
        if ( monstersKilled == getNumMonstersRound())
        {
            roundEnded();
        }
    }

    public void roundEnded()
    {
        monstersKilled = 0;
        waitingForRound = true;
        uIController.roundEnded(() =>
        {
            currentRound++;
            waitingForRound = false;
        }, currentRound);
    }

    IEnumerator Init()
    {
        yield return StartCoroutine(weatherService.Init());
        isRaining = weatherService.IsRaining();
        isNight = weatherService.IsNight();
        isHot = weatherService.IsHot();

        setGameWeather();
    }

    void Update()
    {
        updateMoney(currentDifficultySettings.PassiveMoneyEarned * Time.deltaTime, false);
    }

    public void RemoveDefense(GameObject defense)
    {
        defenses.Remove(defense);
    }

    public void AddDefense(GameObject defense)
    {
        defenses.Add(defense);
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

    public void addDefense(GameObject defense)
    {
        defenses.Add(defense);
    }

    public bool canPlaceDefense()
    {
        return defenses.Count < maxDefenses + 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public bool isWaiting()
    {
        return waitingForRound;
    }
}
