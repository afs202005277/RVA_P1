using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoggerStarter : MonoBehaviour
{
    public TextMeshProUGUI path;

    private void Start()
    {
        Logger.Initialize(path);
    }
}
