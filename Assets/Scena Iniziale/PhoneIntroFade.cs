using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Script che permette di inserire un pannello nero con scritto 'The phone rings' che dissolve dopo 2 secondi e si apre la scena del telefono con il calendario
// con le cose da fare prima di entrare nella città.
public class PhoneIntroFade : MonoBehaviour
{
    public CanvasGroup blackPanel;    // Dove inserire il pannello nero con CanvasGroup
    public float delay = 2f;          // secondi prima del fade alla scena col telefono
    public float fadeDuration = 1.5f; // durata fade dissolvenza

    IEnumerator Start()
    {
        // Aspetta qualche secondo prima di iniziare la dissolvenza
        yield return new WaitForSeconds(delay);

        // Fade-out progressivo da nero a trasparente
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            blackPanel.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        // Devo assicurarmi che il pannello sparisca del tutto per vedere la scena successiva e poter cliccare il bottone
        blackPanel.gameObject.SetActive(false);
    }
}

