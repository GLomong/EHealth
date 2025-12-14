using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartDay1 : MonoBehaviour
{
    // Script che permette se si schiaccia il bottone 'StartTheDay' di fare la transizione da questa scena alla scena della città
    public string nextSceneName = "Città"; 
    public CanvasGroup fadeCanvas; // Canvas per il fade out
    public float fadeDuration = 1f; // Durata del fade

    public void GoToCity()
    {
        StartCoroutine(FadeAndLoad());
    }
    private IEnumerator FadeAndLoad()
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

        SceneManager.LoadScene(nextSceneName);
    }
}