using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFade : MonoBehaviour
{
    public CanvasGroup fadeCanvas;      // Riferimento al CanvasGroup del pannello nero
    public float fadeDuration = 1f;     // Durata della dissolvenza

    private void Start()
    {
        // Quando una scena si apre, parte automaticamente il fade-in
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 1;      // parte da nero
            StartCoroutine(FadeFromBlack());
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
        float t = 0;

        // Fade OUT → da trasparente a nero
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        // Carica la nuova scena
        SceneManager.LoadScene(sceneName);
    }

    // -------------------------
    // FADE IN (AUTOMATICO)
    // -------------------------
    public void FadeIn()
    {
        StartCoroutine(FadeFromBlack());
    }

    private IEnumerator FadeFromBlack()
    {
        float t = 0;

        // Fade IN → da nero a trasparente
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = 0; // fine dissolvenza
        
    }
}


