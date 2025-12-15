using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script per la scena finale: il personaggio prende il taxi che lo porta fuori scena e comincia la giornata 2 (o 3) nuovamente dalla città
public class TaxiTrigger : MonoBehaviour
{
    public GameObject player;              // Riferimento al ragazzo
    public GameObject fineGiornoPanel;     // Pannello UI "Fine giorno 1"
    public TMPro.TextMeshProUGUI endDayText; // Testo da mostrare nel pannello
    public CanvasGroup phoneCanvas;      // canva del telefono
    public float speed = 5f;               // Velocità taxi
    public string nextSceneName = "Città"; // Nome della scena successiva

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && !activated)
        {
            activated = true;

            // 1. Nasconde il ragazzo quando tocca il taxi come se fosse salito sulla macchina
            player.SetActive(false);

            // 2. Attiva la camera per seguire il taxi
            CameraFollowClamp camFollow = Camera.main.GetComponent<CameraFollowClamp>();
            if (camFollow != null)
            {
                camFollow.activeFollow = true;
            }

            // 3. Avvia la coroutine che muove il taxi
            StartCoroutine(MoveTaxiOut());
        }
    }

    private IEnumerator MoveTaxiOut()
    {
        // Calcolo il punteggio totale del giorno e lo salvo 
        SaveScoreForDay();
        int dayScore = PlayerPrefs.GetInt($"Day{TotalGameManager.Instance.CurrentDay}_TotalScore", 0); // punteggio solo minigiochi, pesato
        int totalScore = 0;
        if (TotalGameManager.Instance.CurrentDay >= TotalGameManager.Instance.totalDays)
        {
            totalScore = TotalGameManager.Instance.GetTotalGameScore();
        }

        // 1. Preparo pannello e testo
        CanvasGroup cg = fineGiornoPanel.GetComponent<CanvasGroup>();
        cg.alpha = 0f; // Assicura che il pannello sia trasparente all'inizio
        fineGiornoPanel.SetActive(true);

        Color textColor = endDayText.color;
        textColor.a = 0f;
        endDayText.color = textColor;
        endDayText.text = "End of Day " + TotalGameManager.Instance.CurrentDay + "\nScore: " + dayScore + "/50" + 
                         (totalScore > 0 ? $"\n\nTotal Score: {totalScore}/200" : "");

        float timer = 0f;
        float endAlpha = 0.6f;
        float fadeInDuration = 1f;

        // 2. Il taxi si muove verso destra finché non esce e si avvia il fade in del pannello
        while (transform.position.x < 23f)
        {
            // Movimento del taxi
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);

            // Fade in del pannello e del testo
            if (timer < fadeInDuration)
            {
                phoneCanvas.alpha = Mathf.Lerp(1f, 0f, timer / fadeInDuration); // Fai scomparire il canvas del telefono
                cg.alpha = Mathf.Lerp(0f, endAlpha, timer / fadeInDuration);
                textColor.a = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
                endDayText.color = textColor;
                timer += Time.deltaTime;
            }
            yield return null;
        }

        // Assicura che il pannello e il testo siano completamente visibili
        cg.alpha = endAlpha;
        textColor.a = 1f;
        endDayText.color = textColor;

        // 3. Aggiorna il testo con animazione di scrittura
        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            // Fai scomparire il testo
            textColor.a = Mathf.Lerp(1f, 0f, t / fadeInDuration);
            endDayText.color = textColor;
            yield return null;
        }
        // endDayText.text = "Score: " + PlayerPrefs.GetInt("Day1Score", 0);

        endDayText.text = "Goodnight!";
        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            // Fai apparire il testo
            textColor.a = Mathf.Lerp(0f, 1f, t / fadeInDuration);
            endDayText.color = textColor;
            yield return null;
        }
        // Assicura che il testo sia completamente visibile
        textColor.a = 1f;
        endDayText.color = textColor;

        // 4. Aspetta per un certo tempo
        yield return new WaitForSeconds(0.2f);

        // 5. Fade to black
        float initialAlpha = cg.alpha;
        float textInitialAlpha = textColor.a;

        for (float t = 0; t < fadeInDuration; t += Time.deltaTime)
        {
            // Fai diventare il pannello nero
            cg.alpha = Mathf.Lerp(initialAlpha, 1f, t / fadeInDuration);

            // Fai scomparire il testo
            textColor.a = Mathf.Lerp(textInitialAlpha, 0f, t / fadeInDuration);
            endDayText.color = textColor;

            yield return null;
        }

        // Assicura che sia completamente nero e testo invisibile
        cg.alpha = 1f; // sicuro che sia completamente nero
        textColor.a = 0f;
        endDayText.color = textColor;

        // 6. Carica la scena successiva in base al giorno corrente
        TotalGameManager.Instance.EndDayAndProceed(nextSceneName);
    }

    // Calcolo punteggio alla fine di ogni giorno
    private void SaveScoreForDay()
    {
        int currentDay = TotalGameManager.Instance.CurrentDay;

        // Legge i punteggi dei minigiochi 
        int marketScore = PlayerPrefs.GetInt($"Day{currentDay}_MarketScore", 0);
        int carScore = PlayerPrefs.GetInt($"Day{currentDay}_CarScore", 0);
        int bridgeScore = PlayerPrefs.GetInt($"Day{currentDay}_BridgeScore", 0);
        int danceScore = PlayerPrefs.GetInt($"Day{currentDay}_DanceScore", 0);

        // Legge i punteggi dei dialoghi 
        int cashierScore = PlayerPrefs.GetInt($"Day{currentDay}_CashierScore", 0);
        int influencerScore = PlayerPrefs.GetInt($"Day{currentDay}_InfluencerScore", 0);
        int gentlemenScore = PlayerPrefs.GetInt($"Day{currentDay}_GentlemenScore", 0);
        int friendScore = PlayerPrefs.GetInt($"Day{currentDay}_FriendScore", 0); 

        // Legge i punteggi delle notifiche
        int notifica1Score = PlayerPrefs.GetInt($"Day{currentDay}_Notifica1_Score", 0);
        int notifica2Score = PlayerPrefs.GetInt($"Day{currentDay}_Notifica2_Score", 0);
        int notifica3Score = PlayerPrefs.GetInt($"Day{currentDay}_Notifica3_Score", 0);
    
        // Applica dei pesi ai punteggi dei minigiochi in base al cluster dell'utente
        int cluster = PlayerPrefs.GetInt("UserCluster", 1); // default 1
        float marketWeight = 0.25f;
        float carWeight = 0.25f;
        float bridgeWeight = 0.25f;
        float danceWeight = 0.25f;

        switch(cluster)
        {
            // faccio in modo che i pesi sommati facciano 1 perchè poi non facciamo la media ma la somma pesata
            case 1: // LUCA (droga alto e internet medio)
                marketWeight = 0.2f;
                danceWeight = 0.4f;
                bridgeWeight = 0.05f;
                carWeight = 0.35f;
                break;
            case 2: // PIETRO (alcolismo medio e droga medio)
                marketWeight = 0.3f;
                danceWeight = 0.35f;
                bridgeWeight = 0.1f;
                carWeight = 0.25f;
                break;
            case 3: // ELENA (alcolismo alto e internet medio-basso e gambling alto) 
                marketWeight = 0.4f;
                danceWeight = 0.05f;
                bridgeWeight = 0.3f;
                carWeight = 0.25f;
                break;
            case 4: // FRANCESCO (internet altissimo)
                marketWeight = 0.15f;
                danceWeight = 0.3f;
                bridgeWeight = 0.15f;
                carWeight = 0.4f;
                break;
        }

        // Calcola il punteggio ponderato dei minigiochi (risultato normalizzato 0-50)
        float totalScore = (marketScore * marketWeight) +
                           (carScore * carWeight) +
                           (bridgeScore * bridgeWeight) +
                           (danceScore * danceWeight);
        
        // Calcolo il punteggio totale dei dialoghi (max 2 punti)
        float dialoghiScore = (cashierScore * marketWeight) + 
                                (influencerScore * carWeight) +
                                (gentlemenScore * bridgeWeight) +
                                (friendScore * danceWeight);

        // Calcolo il punteggio totale delle notifiche (max 6 punti)
        float notificheScore = notifica1Score + notifica2Score + notifica3Score;                     

        // Salva il punteggio totale (dei minigiochi) del giorno corrente in PlayerPrefs
        int totalScoreInt = Mathf.RoundToInt(totalScore);
        totalScoreInt = Mathf.Clamp(totalScoreInt, 0, 50);
        PlayerPrefs.SetInt($"Day{currentDay}_TotalScore", totalScoreInt);
        PlayerPrefs.Save();

        // Salva il punteggio totale dei dialoghi del giorno corrente in PlayerPrefs
        PlayerPrefs.SetInt($"Day{currentDay}_DialoguesScore", Mathf.RoundToInt(dialoghiScore));
        PlayerPrefs.Save();

        // Salva il punteggio totale delle notifiche del giorno corrente in PlayerPrefs
        PlayerPrefs.SetInt($"Day{currentDay}_NotificationScore", Mathf.RoundToInt(notificheScore));
        PlayerPrefs.Save();
    }
}
