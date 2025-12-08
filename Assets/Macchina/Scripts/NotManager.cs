using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public GameObject notificationUI;
    public PlayerCar player;

    public float interval = 5f; // ogni 5 secondi dopo che è stata chiusa
    private float timer = 0f;

    void Start()
    {
        if (notificationUI != null)
            notificationUI.SetActive(false); // parte nascosta
    }

    void Update()
    {
        if (!StartScreen.gameStarted)
            return;

        //  Se il gioco è finito:
        if (GameOverUI.gameEnded)
        {
            // se c'è ancora una notifica visibile, la spengo
            if (notificationUI != null && notificationUI.activeSelf)
                notificationUI.SetActive(false);

            // e non faccio altro
            return;
        }

        // se la notifica è visibile, non faccio partire il timer
        if (notificationUI != null && notificationUI.activeSelf)
            return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            ShowNotification();   // riappare notifica
            timer = 0f;           // reset del timer
        }
    }

    void ShowNotification()
    {
        // sicurezza: se il gioco è finito, non mostrare nulla
        if (GameOverUI.gameEnded)
            return;

        if (notificationUI == null)
            return;

        // la rimettiamo al centro ogni volta
        RectTransform rt = notificationUI.GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = Vector2.zero;

        notificationUI.SetActive(true);

        if (player != null)
            player.canMove = false; // blocca la macchina finché non la “scippi”
    }
}


