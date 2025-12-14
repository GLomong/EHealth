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

    public AudioSource audioSource;   //  AudioSource collegato
    public AudioClip phoneRingClip;   //  Clip della suoneria

    [Header("Phone Notification")]
    public CanvasGroup notificationPanel; // Pannello delle notifiche del telefono
    public Animator notif1Animator; // Animator per le notifiche del telefono
    public Animator notif2Animator; // Animator per le notifiche del telefono
    public Animator notif3Animator; // Animator generale per le notifiche
    public string showNotificationTrigger = "ShowNotification"; // Nome del trigger per mostrare la notifica
    IEnumerator Start()
    {
        // Riproduci il suono all'inizio
        if (audioSource != null && phoneRingClip != null)
            audioSource.PlayOneShot(phoneRingClip);
        
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

        // Animazione notifiche sul cellulare
        if (notif1Animator != null)
        {
            notif1Animator.SetTrigger(showNotificationTrigger);
        }
        // wait for a short delay before showing the next notification
        yield return new WaitForSeconds(0.2f);
        if (notif2Animator != null)
        {
            notif2Animator.SetTrigger(showNotificationTrigger);
        }
        // wait for a short delay before showing the next notification
        yield return new WaitForSeconds(0.4f);
        if (notif3Animator != null)
        {
            notif3Animator.SetTrigger(showNotificationTrigger);
        }
    }
}

