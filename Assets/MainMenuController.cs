using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void SetDifficulty()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
