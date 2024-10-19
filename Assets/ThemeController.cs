using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeController : MonoBehaviour
{
    private int currentTheme = 0;
    public GameObject[] themeObjects;

    public void changeDifficulty(int step)
    {
        themeObjects[currentTheme].SetActive(false);
        currentTheme += step;

        if (currentTheme < 0)
        {
            currentTheme = themeObjects.Length-1;
        }
        if (currentTheme == themeObjects.Length) {
            currentTheme = 0;
        }


        themeObjects[currentTheme].SetActive(true);
    }

    public GameObject getCurrentTheme()
    {
        return themeObjects[currentTheme];
    }
}
