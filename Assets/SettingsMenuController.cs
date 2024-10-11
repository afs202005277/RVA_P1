using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;

public class SettingsMenuController : MonoBehaviour
{
    private void Start()
    {
        VuforiaBehaviour.Instance.enabled = true;
    }
    public void back()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void confirm()
    {
        PlayerPrefs.SetInt("difficulty", transform.Find("Difficulty").GetComponent<DifficultyController>().getCurrentDifficulty());
        PlayerPrefs.Save();

        Transform themeTransform = transform.Find("Theme");
        ThemeController themeController = themeTransform.GetComponent<ThemeController>();
        SceneManager.LoadScene(themeController.getCurrentTheme().name);
    }
}
