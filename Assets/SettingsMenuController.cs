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

        PlayerPrefs.SetInt("isAuto", transform.Find("Toggles").GetComponent<ToggleController>().getAuto() ? 1 : 0);
        PlayerPrefs.SetInt("isNight", transform.Find("Toggles").GetComponent<ToggleController>().getNight() ? 1 : 0);
        PlayerPrefs.SetInt("isRain", transform.Find("Toggles").GetComponent<ToggleController>().getRain() ? 1 : 0);
        PlayerPrefs.SetInt("isHot", transform.Find("Toggles").GetComponent<ToggleController>().getHot() ? 1 : 0);
        PlayerPrefs.Save();

        Transform themeTransform = transform.Find("Theme");
        ThemeController themeController = themeTransform.GetComponent<ThemeController>();
        SceneManager.LoadScene(themeController.getCurrentTheme().name);
    }
}
