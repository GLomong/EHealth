using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class BottomReplyPanelManager : MonoBehaviour
{
    public TypewriterEffect typewriter;
    public NotificationsManager notificationsManager; // manager notifiche
    public CanvasGroup telefonoPanel; // pannello telefono
    public TMP_Text reply1LabelText;   // testo dentro il bottone Reply1 (TMP_Text)
    public TMP_Text reply2LabelText;   // testo dentro il bottone Reply2       
    public Button reply1Button;        // bottone Reply1
    public Button reply2Button;        // bottone Reply2  
    public Button xButton;             // bottone X per chiudere il pannello

    public NotificationChoiceUI activeNotification; // chi è la notifica attuale

    public void ShowFor(NotificationChoiceUI notification, string text, string reply1Label, string reply2Label)
    {
        Debug.Log("BottomReplyPanelManager: ShowFor called with text: " + text);
        if (notification != null)
            activeNotification = notification;
        typewriter.ShowText(text);

        // imposta le label dei bottoni in base alla notifica
        if (!string.IsNullOrEmpty(reply1Label))
        {
            reply1LabelText.text = reply1Label;
            reply1Button.gameObject.SetActive(true);
        }
        else
        {
            reply1Button.gameObject.SetActive(false);
        }

        if (!string.IsNullOrEmpty(reply2Label))
        {
            reply2LabelText.text = reply2Label;
            reply2Button.gameObject.SetActive(true);
        }
        else
        {
            reply2Button.gameObject.SetActive(false);
        }
        //xButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void OnReply1Pressed()
    {
        if (activeNotification != null)
            activeNotification.OnReply1Pressed();
        
        // SBLOCCA notifiche
        if (notificationsManager != null)
            notificationsManager.SetWaitingForChoice(false);

        ClosePanelAndCheckNotifications();  // chiudi + controlla se finale
        if (notificationsManager != null && notificationsManager.allAnswered) 
        {
            notificationsManager.LastText();
        }
    }

    public void OnReply2Pressed()
    {
        if (activeNotification != null)
            activeNotification.OnReply2Pressed();
        
        // SBLOCCA notifiche
        if (notificationsManager != null)
            notificationsManager.SetWaitingForChoice(false);

        ClosePanelAndCheckNotifications();  // chiudi + controlla se finale
        if (notificationsManager != null && notificationsManager.allAnswered) 
        {
            notificationsManager.LastText();
        }
    }

    public void OnXPressed()
    {
        // SBLOCCA notifiche
        if (notificationsManager != null)
            notificationsManager.SetWaitingForChoice(false);
            
        Debug.Log("BottomReplyPanelManager: OnXPressed");

        // SE è il messaggio finale → fai fade del telefono
        if (notificationsManager != null && notificationsManager.allAnswered)
        {
            notificationsManager.LastText();
            notificationsManager.canFade = true;
            Debug.Log("BottomReplyPanelManager: OnXPressed - last text");
            notificationsManager.allAnswered = false;
        }
        else if (notificationsManager.canFade)
        {
            StartCoroutine(FadePanelCoroutine());
            // Chiudi pannello e notifica attiva
            Debug.Log("BottomReplyPanelManager: OnXPressed - closing active notification panel");
            notificationsManager.canFade = false;
        }
        else
        {
            // Altrimenti chiudi normalmente
            gameObject.SetActive(false);
            reply1Button.gameObject.SetActive(false);
            reply2Button.gameObject.SetActive(false);
            xButton.gameObject.SetActive(false);
        }
    }
    IEnumerator FadePanelCoroutine()
    {
        float fadeDuration = 1.5f;
        // Fade-out progressivo dello schermo del telefono
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            telefonoPanel.alpha = 1 - (t / fadeDuration);
            yield return null;
        }

        // Devo assicurarmi che il pannello sparisca del tutto per vedere la scena successiva e poter cliccare il bottone
        telefonoPanel.gameObject.SetActive(false);

        gameObject.SetActive(false);
        reply1Button.gameObject.SetActive(false);
        reply2Button.gameObject.SetActive(false);
        xButton.gameObject.SetActive(false);
    }

    public void ShowFinalMessage(string finalText)
    {
        activeNotification = null;  // niente notifica attiva
        typewriter.ShowText(finalText);

        // Nascondi TUTTI i bottoni tranne X
        reply1Button.gameObject.SetActive(false);
        reply2Button.gameObject.SetActive(false);
        xButton.gameObject.SetActive(true);  // X sempre visibile per finale

        gameObject.SetActive(true);
    }

    void ClosePanelAndCheckNotifications()
    {
        // Chiudi pannello
        gameObject.SetActive(false);
        reply1Button.gameObject.SetActive(false);
        reply2Button.gameObject.SetActive(false);
        xButton.gameObject.SetActive(false);

        // SE NON è già il messaggio finale, controlla se mostrarlo
        if (notificationsManager != null && !notificationsManager.allAnswered)
        {
            notificationsManager.RegisterNotificationAnswered();
        }
    }
}


