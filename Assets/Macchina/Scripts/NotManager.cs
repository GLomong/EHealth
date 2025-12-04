using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    public GameObject notificationUI;
    public PlayerCar player;

    public float interval = 5f;   //ogni 5 secondi dopo che è stata chiusa

    private float timer = 0f; //contatore del tempo passato

    void Start() //all’inizio nasconde la notifica
    {
        if (notificationUI != null)
            notificationUI.SetActive(false);   // parte nascosta
    }

    void Update()
    {
        // se la notifica è visibile, non faccio partire il timer
        if (notificationUI != null && notificationUI.activeSelf)
            return;

        timer += Time.deltaTime;

        if (timer >= interval)
        {
            ShowNotification(); //riappare notifica
            timer = 0f; //reset del timer
        }
    }

    void ShowNotification()
    {
        if (notificationUI == null) return;

        // la rimettiamo al centro ogni volta
        RectTransform rt = notificationUI.GetComponent<RectTransform>();
        if (rt != null)
            rt.anchoredPosition = Vector2.zero;

        notificationUI.SetActive(true);

        if (player != null)
            player.canMove = false; //blocca la macchina
    }
}
