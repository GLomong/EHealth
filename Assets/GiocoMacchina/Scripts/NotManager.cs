using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public GameObject notificationUI;
    public PlayerCar player;

    public float interval = 5f; // ogni 5 secondi dopo che è stata chiusa
    private float timer = 0f;

    void Start()
    {
        // ricavo il cluster dell'utente 
        int cluster = PlayerPrefs.GetInt("ClusterUtente", 1);

        // regolo l'autoSpawnInterval in base al cluster (cluster salvati da 1 a 4)
        switch (cluster)
        {
            case 1: // LUCA (internet 64.5/100)
                // notifiche più freqeunti 
                interval = 3.0f;
                break;
            case 2: // PIETRO (internet 16/100)
                // notifiche normali
                interval = 5.0f;
                break;
            case 3: // ELENA (internet 39/100)
                // notifiche leggermente piu frequenti
                interval = 4.0f;
                break;
            case 4: // FRANCESCO (internet 91/10)
                // notifiche molto piu frequenti
                interval = 2.0f;
                break;
        }
        Debug.Log($"Cluster {cluster} → interval={interval}");

        // incremento la difficoltà in base al giorno
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        float difficultyIncrement = 0.05f * (currentDay - 1); // primo giorno = 0, secondo = 0.05, terzo = 0.10
        interval = Mathf.Max(0.2f, interval - difficultyIncrement);
        Debug.Log($"Giorno {currentDay} → interval={interval}");

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


