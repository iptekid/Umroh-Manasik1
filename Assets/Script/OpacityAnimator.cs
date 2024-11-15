using UnityEngine;
using System.Collections;

public class OpacityAnimator : MonoBehaviour
{
    private Material material;
    private float currentOpacity = 1f;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        // Get the material from the renderer
        material = GetComponent<Renderer>().material;
    }

    // Fade to target opacity over duration
    public void FadeTo(float targetOpacity, float duration)
    {
        // Stop any existing fade
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeRoutine(targetOpacity, duration));
    }

    // Fade in (to full opacity)
    public void FadeIn(float duration)
    {
        FadeTo(1f, duration);
    }

    // Fade out (to transparent)
    public void FadeOut(float duration)
    {
        FadeTo(0f, duration);
    }

    private IEnumerator FadeRoutine(float targetOpacity, float duration)
    {
        float startOpacity = material.GetFloat("_Opacity");
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float normalizedTime = elapsed / duration;

            // Lerp the opacity
            float currentOpacity = Mathf.Lerp(startOpacity, targetOpacity, normalizedTime);
            material.SetFloat("_Opacity", currentOpacity);

            yield return null;
        }

        // Ensure we end up exactly at the target opacity
        material.SetFloat("_Opacity", targetOpacity);
        currentOpacity = targetOpacity;
    }

    // Instantly set opacity
    public void SetOpacity(float opacity)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        opacity = Mathf.Clamp01(opacity);
        material.SetFloat("_Opacity", opacity);
        currentOpacity = opacity;
    }

    // Optional: Ping-pong animation between two opacity values
    public void StartPingPong(float minOpacity, float maxOpacity, float duration)
    {
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

    // Stop any ongoing animation
    public void StopAnimation()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }
    }
}