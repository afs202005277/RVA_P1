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
    public List<GameObject> destroyedDefenses;
    public List<GameObject> monsterPrefabs;
    private float currentMoney = 100;
    
    public int currentRound = 1;
    private bool waitingForRound = false;

    private int monstersKilled = 0;
    public int monsterPerRound;

    public DifficultyConfig difficultyConfig;

    public int maxDefenses;

    public GameObject currentCastle;

    private float _growthFactor = 1.1f;

    void Start()
    {
        Time.timeScale = 1f;
        /*if (PlayerPrefs.GetInt("isAuto", 0) == 1 && WeatherServiceEnabled)
        {
            StartCoroutine(Init());
        }
        else
        {*/
        isNight = PlayerPrefs.GetInt("isNight", 0) == 1;
        isRaining = PlayerPrefs.GetInt("isRain", 0) == 1;
        isHot = PlayerPrefs.GetInt("isHot", 0) == 1;
        //}

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
        return (int)((monsterPerRound + currentRound - 1) * Math.Pow(_growthFactor, currentRound - 1));
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
        currentCastle.GetComponent<BaseController>().health = currentCastle.GetComponent<BaseController>().maxHealth;
        destroyedDefenses.ForEach(defense => defense.GetComponent<DefensiveStructure>().health = defense.GetComponent<DefensiveStructure>().maxHealth);
        defenses.ForEach(defense => {
            if (defense.GetComponent<DefensiveStructure>())
                defense.GetComponent<DefensiveStructure>().health = defense.GetComponent<DefensiveStructure>().maxHealth;
            });
        destroyedDefenses.ForEach(defenses => defenses.GetComponent<DefensiveStructure>().repair());
        destroyedDefenses.Clear();
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

    public void DestroyDefense(GameObject defense)
    {
        destroyedDefenses.Add(defense);
    }
}
