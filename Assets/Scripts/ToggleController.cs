using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    private bool _isAuto = true;
    private bool _isNight = false;
    private bool _isRain = false;
    private bool _isHot = false;

    public void auto()
    {
        if (_isAuto) _isAuto = false;
        else _isAuto = true;

    }

    public void night()
    {
        if (_isNight) _isNight = false;
        else _isNight = true;
    }

    public void rain()
    {
        if (_isRain) _isRain = false;
        else _isRain = true;

    }

    public void hot()
    {
        if (_isHot) _isHot = false;
        else _isHot = true;

    }

    public bool getAuto()
    {
        return _isAuto;
    }

    public bool getNight()
    {
        return _isNight;
    }

    public bool getRain()
    {
        return _isRain;
    }

    public bool getHot()
    {
        return _isHot;
    }
}
