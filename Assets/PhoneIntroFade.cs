using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PhoneIntroFade : MonoBehaviour
{
    public CanvasGroup blackPanel;    // il pannello nero con CanvasGroup
    public float delay = 2f;          // secondi prima del fade
    public float fadeDuration = 1.5f; // durata fade

    IEnumerator Start()
    {
        // Aspetta qualche secondo
        yield return new WaitForSeconds(delay);

        // Fade-out progressivo
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            blackPanel.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        // Assicurati che sparisca del tutto
        blackPanel.gameObject.SetActive(false);
    }
}

