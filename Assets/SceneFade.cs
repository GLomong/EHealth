using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;   

public class SceneFade : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        // Fade IN (da trasparente → nero)
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            yield return null;
        }

        // Carica la scena
        SceneManager.LoadScene(sceneName);
    }
}

