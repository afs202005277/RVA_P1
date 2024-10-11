using Mono.Unix.Native;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyController : MonoBehaviour
{
    private int currentDifficulty = 0;
    public GameObject[] difficultyObjects;

    public void changeDifficulty(int step)
    {
        difficultyObjects[currentDifficulty].SetActive(false);
        currentDifficulty += step;
        if (currentDifficulty < 0)
        {
            currentDifficulty = difficultyObjects.Length - 1;
        }
        if (currentDifficulty == difficultyObjects.Length)
        {
            currentDifficulty = 0;
        }
        difficultyObjects[currentDifficulty].SetActive(true);
    }

    public int getCurrentDifficulty()
    {
        return currentDifficulty;
    }
}
