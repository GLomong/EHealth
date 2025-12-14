using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class NotificationChoiceUI : MonoBehaviour
{
    public BottomReplyPanelManager bottomReplyPanelManager;   // manager pannello in basso
    public NotificationsManager notificationsManager; // manager notifiche
    [TextArea] public string replyText;   // testo quando premi Reply
    [TextArea] public string ignoreText;  // testo quando premi Ignore
    public TMP_Text notificationText;   // il testo principale della notifica (quello fisso)
    [TextArea] public string reply1Label;   // testo da scrivere su reply1
    [TextArea] public string reply2Label;   // testo da scrivere su reply2
    
    // cosa diventa il testo della notifica dopo reply1 / reply2
    [TextArea] public string reply1NotificationText;
    [TextArea] public string reply2NotificationText;

    public Button replyButton;  // bottone Reply
    public Button ignoreButton; // bottone Ignore
    public Button xButton;      // bottone X per chiudere la notifica

    public Animator notificationAnimator; // Animator per la notifica
    public string showNotificationTrigger = "ShowNotification";

    bool hasChoosen = false;

    public void OnReplyPressed()
    {
        // BLOCCO: non permettere se già in attesa di scelta da un'altra notifica
        if (notificationsManager != null && notificationsManager.waitingForChoice)
        {
            Debug.Log($"[] Blocked: altra notifica in attesa di scelta");
            return;
        }

        if (hasChoosen) return;
        hasChoosen = true;

        // Blocca tutte le altre notifiche
        if (notificationsManager != null)
            notificationsManager.SetWaitingForChoice(true);

        // disattiva i bottoni Reply e Ignore
        replyButton.interactable = false;
        ignoreButton.interactable = false;

        // mostra testo di reply e label bottoni
        bottomReplyPanelManager.ShowFor(this, replyText, reply1Label, reply2Label);
        xButton.gameObject.SetActive(false);
    }

    public void OnIgnorePressed()
    {
        if (notificationsManager != null && notificationsManager.waitingForChoice)
        {
            Debug.Log($"[] Blocked: altra notifica attiva");
            return;
        }

        if (hasChoosen) return;
        hasChoosen = true;

        // Salva il punteggio Ignore (Neutrale) = 1 punto
        SaveNotificationScore(1);

        // Blocca tutte le altre notifiche
        if (notificationsManager != null)
            notificationsManager.SetWaitingForChoice(true);

        // disattiva i bottoni Reply e Ignore
        replyButton.interactable = false;
        ignoreButton.interactable = false;

        // mostra testo di ignore
        bottomReplyPanelManager.ShowFor(this, ignoreText, "", "");
        xButton.gameObject.SetActive(true);
        
        if (notificationsManager != null)
            notificationsManager.RegisterNotificationAnswered();
    }

    public void OnReply1Pressed()
    {
        // Salva il punteggio Reply1 (Negativa) = 0 punti
        SaveNotificationScore(0);

        // Cambia il testo della notifica (quella da cui è partito il Reply)
        if (notificationText != null && !string.IsNullOrEmpty(reply1NotificationText))
        {
            replyButton.gameObject.SetActive(false);
            ignoreButton.gameObject.SetActive(false);
            notificationText.text = reply1NotificationText;

            PlayAnimation();
        }
    }

    public void OnReply2Pressed()
    {
        // Salva il punteggio Reply2 (Positiva) = 2 punti
        SaveNotificationScore(2);

        if (notificationText != null && !string.IsNullOrEmpty(reply2NotificationText))
        {
            replyButton.gameObject.SetActive(false);
            ignoreButton.gameObject.SetActive(false);
            notificationText.text = reply2NotificationText;

            PlayAnimation();
        }
    }

    void PlayAnimation()
    {
        // Porto lo scaler a 0 e poi faccio partire animazione
        transform.localScale = Vector3.zero;

        // riproduci animazione notifica 
        if (notificationAnimator != null)
        {
            notificationAnimator.SetTrigger(showNotificationTrigger);
        }
    }

    void SaveNotificationScore(int score)
    {
        int currentDay = TotalGameManager.Instance.CurrentDay;
        string notificationKey = $"Day{currentDay}_{gameObject.name}_Score";
        
        PlayerPrefs.SetInt(notificationKey, score);
        PlayerPrefs.Save();
        
        Debug.Log($"[{gameObject.name}] Salvato Day{currentDay} Score = {score} in '{notificationKey}'");
    }
}
