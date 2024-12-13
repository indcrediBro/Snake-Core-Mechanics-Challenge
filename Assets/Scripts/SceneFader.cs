using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : Singleton<SceneFader>
{
    [SerializeField] private Image fadeOverlay;
    [SerializeField] private float fadeDuration = 1f;

    public IEnumerator FadeIn()
    {
        float timer = 0f;
        Color fadeColor = fadeOverlay.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeOverlay.color = fadeColor;
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        float timer = 0f;
        Color fadeColor = fadeOverlay.color;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            fadeOverlay.color = fadeColor;
            yield return null;
        }
    }
}
