using UnityEngine;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Common interface for opacity animation

public class UIOpacityAnimator : MonoBehaviour, IOpacityAnimator
{
    private CanvasGroup canvasGroup;
    private float currentOpacity = 1f;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            Debug.LogError("UIOpacityAnimator requires a CanvasGroup component");
        }
    }

    public void FadeTo(float targetOpacity, float duration)
    {
        if (canvasGroup == null) return;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeRoutine(targetOpacity, duration));
    }

    public void FadeIn(float duration)
    {
        FadeTo(1f, duration);
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }
    }

    public void FadeOut(float duration)
    {
        FadeTo(0f, duration);
        if (canvasGroup != null)
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }

    private IEnumerator FadeRoutine(float targetOpacity, float duration)
    {
        float startOpacity = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;
            float currentOpacity = Mathf.Lerp(startOpacity, targetOpacity, normalizedTime);
            canvasGroup.alpha = currentOpacity;
            yield return null;
        }

        canvasGroup.alpha = targetOpacity;
        currentOpacity = targetOpacity;
    }

    public void SetOpacity(float opacity)
    {
        if (canvasGroup == null) return;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        opacity = Mathf.Clamp01(opacity);
        canvasGroup.alpha = opacity;
        canvasGroup.blocksRaycasts = opacity > 0;
        canvasGroup.interactable = opacity > 0;
        currentOpacity = opacity;
    }

    public void Show()
    {
        SetOpacity(1f);
    }

    public void Hide()
    {
        SetOpacity(0f);
    }

    public void StartPingPong(float minOpacity, float maxOpacity, float duration)
    {
        if (canvasGroup == null) return;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(PingPongRoutine(minOpacity, maxOpacity, duration));
    }

    private IEnumerator PingPongRoutine(float minOpacity, float maxOpacity, float duration)
    {
        while (true)
        {
            yield return StartCoroutine(FadeRoutine(maxOpacity, duration / 2));
            yield return StartCoroutine(FadeRoutine(minOpacity, duration / 2));
        }
    }

    public void StopAnimation()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }
}
