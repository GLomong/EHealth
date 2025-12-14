using UnityEngine;
using UnityEngine.UI;

public class NotificationsManager : MonoBehaviour
{
    public int totalNotifications = 3;      // quante notifiche in totale
    public int answeredCount = 0;
    [HideInInspector] public bool allAnswered = false;  // stato chiaro
    [HideInInspector] public bool canFade = false;
    [HideInInspector] public bool waitingForChoice = false;


    public BottomReplyPanelManager bottomReplyPanelManager;
    public Button xButton;      // bottone X per chiudere il pannello
    [TextArea] public string allAnsweredText;  // testo finale da mostrare nel bottom panel

    public void RegisterNotificationAnswered()
    {
        answeredCount++;
        Debug.Log($"Answered notifications: {answeredCount}/{totalNotifications}");
        if (answeredCount >= totalNotifications)
        {
            allAnswered = true;
        }
    }
    public void LastText()
    {
        // tutte le notifiche hanno ricevuto una risposta
        if (bottomReplyPanelManager != null && !string.IsNullOrEmpty(allAnsweredText))
        {
            Debug.Log("NotificationsManager: LastText called, showing final message.");
            bottomReplyPanelManager.ShowFor(null, allAnsweredText, "", "");
            xButton.gameObject.SetActive(true);
            allAnswered = false; // reset stato
            canFade = true;
        }
    }
    public void SetWaitingForChoice(bool waiting)
    {
        waitingForChoice = waiting;
        Debug.Log($"NotificationsManager: waitingForChoice set to {waitingForChoice}");
    }
}

