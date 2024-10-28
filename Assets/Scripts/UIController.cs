using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public float blinkDuration;       // Total time for blinking 10
    public int blinkCount;             // How many times it should blink 10
    public CanvasGroup warningsCanvas;

    public TextMeshProUGUI roundIndicator;
    public TextMeshProUGUI roundText;

    public GameObject upgradeButton;

    private int frameCounter = 0;
    private const int updateInterval = 4;

    public TextMeshProUGUI moneytext;

    public String notEnoughMoneyMessage;
    public String maxDefensesReachedMessage;
    public String maxBaseLevelReachedMessage;

    public void notEnoughMoney()
    {
        TextMeshProUGUI text = warningsCanvas.GetComponentInChildren<TextMeshProUGUI>();
        text.text = notEnoughMoneyMessage;
        StartCoroutine(FadeInAndOut(warningsCanvas, 0.5f));
    }

    public void maxCastleLevel()
    {
        TextMeshProUGUI text = warningsCanvas.GetComponentInChildren<TextMeshProUGUI>();
        text.text = maxBaseLevelReachedMessage;
        StartCoroutine(FadeInAndOut(warningsCanvas, 0.5f));
    }

    public void maxDefensesReached()
    {
        TextMeshProUGUI text = warningsCanvas.GetComponentInChildren<TextMeshProUGUI>();
        text.text = maxDefensesReachedMessage;
        StartCoroutine(FadeInAndOut(warningsCanvas, 0.5f));
    }

    public void roundEnded(Action onBlinkCompleteCallback, int currentRound)
    {
        Debug.Log("AFSDEBUGGING: Round ended");
        StartCoroutine(BlinkRoundIndicator(blinkDuration, blinkCount, onBlinkCompleteCallback, currentRound));
    }

    IEnumerator BlinkRoundIndicator(float duration, int count, Action onBlinkCompleteCallback, int currentRound)
    {
        upgradeButton.transform.GetChild(2).gameObject.SetActive(false);
        upgradeButton.GetComponent<UnityEngine.UI.Button>().enabled = true;
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
        onBlinkCompleteCallback.Invoke();
        roundIndicator.text = (currentRound+1).ToString();
        upgradeButton.transform.GetChild(2).gameObject.SetActive(true);
        upgradeButton.GetComponent<UnityEngine.UI.Button>().enabled = false;
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

    private IEnumerator FadeInAndOut(CanvasGroup canvasGroup, float fadeDuration)
    {
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 0, 1, fadeDuration));

        // Keep the canvas visible for a short time
        yield return new WaitForSeconds(0.5f);

        // Fade out and deactivate
        yield return StartCoroutine(FadeCanvasGroup(canvasGroup, 1, 0, fadeDuration));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }

    public void updateMoneyDisplayed(float currentMoney, bool overrideUpdate)
    {
        frameCounter++;
        if (frameCounter >= updateInterval)
        {
            moneytext.text = ((float)Math.Round(currentMoney, 0)).ToString();
            frameCounter = 0;
        }
    }
    
}
