using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DifficultyConfig
{
    public enum Difficulty { Easy, Medium, Hard }
    public DifficultySettings EasySettings = new DifficultySettings(
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
        1.2f, // Passive money earned for Easy
        100.0f, // cost of wall
        150.0f // cost of tower
    );
    public DifficultySettings MediumSettings = new DifficultySettings(
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
        100.0f, // Passive money earned for Medium
        120.0f, // cost of wall
        170.0f // cost of tower
    );
    public DifficultySettings HardSettings = new DifficultySettings(
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
        0.8f, // Passive money earned for Hard
        150.0f, // cost of wall
        200.0f // cost of tower
    );

    public DifficultySettings GetSettings(int difficulty)
    {
        Difficulty enumDifficulty = (Difficulty)PlayerPrefs.GetInt("difficulty", 1);
        switch (enumDifficulty)
        {
            case Difficulty.Easy:
                return EasySettings;
            case Difficulty.Medium:
                return MediumSettings;
            case Difficulty.Hard:
                return HardSettings;
            default:
                throw new System.ArgumentOutOfRangeException(nameof(difficulty), difficulty, null);
        }
    }
}
