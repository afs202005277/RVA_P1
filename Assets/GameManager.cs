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

    public enum Difficulty { Easy, Medium, Hard }
    public TextMeshProUGUI moneytext;
    private DifficultySettings currentDifficultySettings;
    public List<GameObject> defenses;
    public List<GameObject> monsterPrefabs;
    private float currentMoney = 0;
    private int frameCounter = 0;
    private const int updateInterval = 4;
    private bool overrideUpdate = false;
    private int currentRound = 1;
    public TextMeshProUGUI roundIndicator;
    public TextMeshProUGUI roundText;
    public float blinkDuration = 10f;       // Total time for blinking
    public int blinkCount = 5;             // How many times it should blink

    private int monstersKilled = 0;
    public int monsterPerRound = 3;

    void Start()
    {
        if (WeatherServiceEnabled)
        {
            StartCoroutine(Init());
        }
        
        currentDifficultySettings = getSettings((Difficulty)PlayerPrefs.GetInt("difficulty", 1));

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
        StartCoroutine(BlinkRoundIndicator(blinkDuration, blinkCount));
    }

    IEnumerator BlinkRoundIndicator(float duration, int count)
    {
        float fadeTime = duration / (count * 2); // Time for each fade in/out
        Color originalColor = roundIndicator.color;
        Color transparentColor = originalColor;
        transparentColor.a = 0;  // Set alpha to 0 for fade out

        for (int i = 0; i < count; i++)
        {
            // Fade out
            yield return StartCoroutine(FadeTo(transparentColor, fadeTime));

            // Fade in
            yield return StartCoroutine(FadeTo(originalColor, fadeTime));
        }
        OnBlinkComplete();
    }

    void OnBlinkComplete()
    {
        currentRound++;
        roundIndicator.text = currentRound.ToString();
    }

    IEnumerator FadeTo(Color targetColor, float duration)
    {
        Color startColor = roundIndicator.color;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            roundIndicator.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            roundText.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        roundIndicator.color = targetColor;
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
        frameCounter++;
        currentMoney += currentDifficultySettings.PassiveMoneyEarned * Time.deltaTime;
        if (frameCounter >= updateInterval | overrideUpdate)
        {
            moneytext.text = ((float)Math.Round(currentMoney, 0)).ToString();
            frameCounter = 0;
            overrideUpdate = false;
        }
    }

    public void RemoveDefense(GameObject defense)
    {
        defenses.Remove(defense);
    }

    private DifficultySettings getSettings(Difficulty difficulty)
    {
        Dictionary<Difficulty, DifficultySettings>  difficultySettings = new Dictionary<Difficulty, DifficultySettings>
{
    { Difficulty.Easy, new DifficultySettings(
        new DefensiveStructureSettings(1.2f, 1.2f, 1.2f, 1.1f), // Tower and Wall settings for Easy
        new Dictionary<string, MonsterSettings>                 // Monster settings per prefab for Easy
        {
            { "Zombie", new MonsterSettings(0.9f, 0.9f, 0.9f, 50f) },
            { "Lizard", new MonsterSettings(0.85f, 0.85f, 0.85f, 45f) },
            { "Ogre", new MonsterSettings(0.8f, 0.8f, 0.8f, 70f) },
            { "Scavenger", new MonsterSettings(0.95f, 0.95f, 0.95f, 40f) },
            { "Chomper", new MonsterSettings(0.9f, 0.9f, 0.9f, 55f) },
            { "Spitter", new MonsterSettings(0.9f, 0.9f, 0.9f, 30f) }
        },
        1.2f // Passive money earned for Easy
    )},

    { Difficulty.Medium, new DifficultySettings(
        new DefensiveStructureSettings(1.0f, 1.0f, 1.0f, 1.0f), // Tower and Wall settings for Medium
        new Dictionary<string, MonsterSettings>                 // Monster settings per prefab for Medium
        {
            { "Zombie", new MonsterSettings(1.0f, 1.0f, 1.0f, 50f) },
            { "Lizard", new MonsterSettings(1.0f, 1.0f, 1.0f, 45f) },
            { "Ogre", new MonsterSettings(1.0f,1.0f, 1.0f, 70f) },
            { "Scavenger", new MonsterSettings(1.0f, 1.0f, 1.0f, 40f) },
            { "Chomper", new MonsterSettings(1.0f, 1.0f, 1.0f, 55f) },
            { "Spitter", new MonsterSettings(1.0f, 1.0f, 1.0f, 30f) }
        },
        1.0f // Passive money earned for Medium
    )},

    { Difficulty.Hard, new DifficultySettings(
        new DefensiveStructureSettings(0.8f, 0.8f, 0.8f, 0.9f), // Tower and Wall settings for Hard
        new Dictionary<string, MonsterSettings>                 // Monster settings per prefab for Hard
        {
            { "Zombie", new MonsterSettings(1.2f, 1.2f, 1.2f, 50f) },
            { "Lizard", new MonsterSettings(1.15f, 1.15f, 1.15f, 45f) },
            { "Ogre", new MonsterSettings(1.1f, 1.1f, 1.1f, 70f) },
            { "Scavenger", new MonsterSettings(1.05f, 1.05f, 1.05f, 40f) },
            { "Chomper", new MonsterSettings(1.1f, 1.1f, 1.1f, 55f) },
            { "Spitter", new MonsterSettings(1.1f, 1.1f, 1.1f, 30f) }
        },
        0.8f // Passive money earned for Hard
    )}
};
        return difficultySettings[difficulty];
    }

    public DifficultySettings CurrentDifficultySettings
    {
        get { return currentDifficultySettings; }
    }

    public void addMoney(float coins)
    {
        this.currentMoney += coins;
        overrideUpdate = true;
    }
}
