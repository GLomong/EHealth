using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    private void Start()
    {
        // NON partire da nero automaticamente.
        // Assicuriamo solo che il pannello sia trasparente
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0;
        }
    }

    // -------------------------
    // FADE OUT + CAMBIO SCENA
    // -------------------------
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
            yield return null;
        }

        fadeCanvas.alpha = 1;

        SceneManager.LoadScene(sceneName);
    }

    // -------------------------
    // FADE MANUALE DOPO IL CARICAMENTO (se serve)
    // -------------------------
    public void FadeIn()
    {
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 0;
    }
}