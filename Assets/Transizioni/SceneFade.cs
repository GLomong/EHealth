using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public CanvasGroup phoneCanvas;
    public float fadeDuration = 1f;

    private void Start()
    {
        // Parte da nero e sischiarisce fino a trasparente
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 1;
            if (phoneCanvas != null)
                phoneCanvas.alpha = 0;
            StartCoroutine(FadeFromBlack());
        }
    }

    // FADE OUT + CAMBIO SCENA
    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        fadeCanvas.blocksRaycasts = true;
        fadeCanvas.interactable = false;

        float t = 0;
        float startAlpha = fadeCanvas.alpha;
        float endAlpha = 1f;

        // Fade OUT → trasparente verso nero
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, endAlpha, Mathf.SmoothStep(0, 1, t / fadeDuration));
            if (phoneCanvas != null)
                phoneCanvas.alpha = Mathf.Lerp(1, 0, Mathf.SmoothStep(0, 1, t / fadeDuration));
            yield return null;
        }

        fadeCanvas.alpha = 1;

        SceneManager.LoadScene(sceneName);
    }

    // FADE di ritorno da black a trasparente
    public void FadeIn()
    {
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        Debug.Log("[SceneFade] Inizio Fade From Black");
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            if (phoneCanvas != null)
                phoneCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 0;
    }
}