using UnityEngine;
using UnityEngine.SceneManagement;

public class TotalGameManager : MonoBehaviour
{
    public static TotalGameManager Instance { get; private set; }

    [Header("Game settings")]
    public int totalDays = 3;
    public string finalSceneName = "FinalScene";

    private const string PlayerPrefsCurrentDayKey = "CurrentDay";

    public int CurrentDay { get; private set; } = 1;

    private void Start()
    {
        // Se esiste già un giorno, se è pari a 3 resetto a 1 (per sicurezza)
        if (PlayerPrefs.HasKey(PlayerPrefsCurrentDayKey))
        {
            int savedDay = PlayerPrefs.GetInt(PlayerPrefsCurrentDayKey, 1);
            if (savedDay >= totalDays)
            {
                ResetAllProgress();
            }
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Carica giorno corrente (default 1)
        CurrentDay = PlayerPrefs.GetInt(PlayerPrefsCurrentDayKey, 1);
    }

    // Da chiamare dalla scena del taxi
    public void EndDayAndProceed(string nextSceneName)
    {
        if (CurrentDay >= totalDays)
        {
            // Se è fintio il terzo giorno, carica la scena finale a seconda del punteggio totale del gioco 
            int totalScore = GetTotalGameScore();
            Debug.Log($"Punteggio totale del gioco: {totalScore}");
            if (totalScore >= 100)
            {
                finalSceneName = "Finale Positivo";
            }
            else if (totalScore < 100)
            {
                finalSceneName = "Finale Negativo";
            }
            SceneManager.LoadScene(finalSceneName);
            return;
        }

        // Avanza di un giorno
        CurrentDay++;
        PlayerPrefs.SetInt(PlayerPrefsCurrentDayKey, CurrentDay);
        PlayerPrefs.Save();

        // Carica scena successiva
        SceneManager.LoadScene(nextSceneName);
    }

    // Ritorna il punteggio di un minigioco specifico per un certo giorno
    public int GetMiniGameScore(string miniGameName, int day)
    {
        return PlayerPrefs.GetInt($"Day{day}_{miniGameName}", 0);
    }

    // Calcolare la somma totale del gioco
    public int GetTotalGameScore()
    {
        int total = 0;
        int minigiochi = 0;
        int dialoghi = 0;
        int notifiche = 0;
        int narrazione = 0;
        for (int day = 1; day <= totalDays; day++)
        {
            // Somma risultati dei vari giorni 
            minigiochi += PlayerPrefs.GetInt($"Day{day}_TotalScore", 0); // punteggio minigiochi, massimo 50 al giorno
            dialoghi += PlayerPrefs.GetInt($"Day{day}_DialoguesScore", 0); // punteggio dialoghi, massimo 2 al giorno
            notifiche += PlayerPrefs.GetInt($"Day{day}_NotificationScore", 0); // punteggio notifiche, massimo 6 al giorno
        }
        // Normalizzazione punteggio dialoghi e notifiche
        narrazione = (dialoghi + notifiche); // contando che le notifiche ci sono solo al giorno 1, totale massimo di 12 punti
        minigiochi = Mathf.RoundToInt((minigiochi / (float)(totalDays * 50))*100); // scale 0-100 
        narrazione = Mathf.RoundToInt((narrazione / (float)((totalDays * 2)+6))*100); // scale 0-100

        // Somma totale (massimo 200 punti)
        total = minigiochi + narrazione;

        // Analisi trend dei punteggi (narrazione)
        int previousDayNarrationScore = 0;
        for (int day = 1; day <= totalDays; day++)
        {
            int currentDayDialogues = PlayerPrefs.GetInt($"Day{day}_DialoguesScore", 0);
            int currentDayNotifications = PlayerPrefs.GetInt($"Day{day}_NotificationScore", 0);
            int currentDayNarrationScore = currentDayDialogues + currentDayNotifications;
            if (day > 1)
            {
                if (currentDayNarrationScore > previousDayNarrationScore)
                {
                    total += 5; // bonus di 5 punti per miglioramento
                    Debug.Log($"Day {day}: Miglioramento nel punteggio di narrazione rispetto al giorno precedente.");
                }
                else if (currentDayNarrationScore < previousDayNarrationScore)
                {
                    total -= 5; // penalità di 5 punti per peggioramento
                    Debug.Log($"Day {day}: Peggioramento nel punteggio di narrazione rispetto al giorno precedente.");
                }
                else
                {
                    // punteggio invariato, nessuna variazione
                    Debug.Log($"Day {day}: Nessuna variazione nel punteggio di narrazione rispetto al giorno precedente.");
                }
            }
            previousDayNarrationScore = currentDayNarrationScore;
        }
        return total;
    }

    // Reset generale (utile per test)
    public void ResetAllProgress()
    {
        CurrentDay = 1;
        PlayerPrefs.SetInt(PlayerPrefsCurrentDayKey, 1);

        // Cancella cluster utente
        PlayerPrefs.DeleteKey("ClusterUtente");
        // Cancella tutti i punteggi (minigiochi e dialoghi)
        for (int day = 1; day <= totalDays; day++)
        {
            PlayerPrefs.DeleteKey($"Day{day}_TotalScore");
            PlayerPrefs.DeleteKey($"Day{day}_MarketScore");
            PlayerPrefs.DeleteKey($"Day{day}_CarScore");
            PlayerPrefs.DeleteKey($"Day{day}_BridgeScore");
            PlayerPrefs.DeleteKey($"Day{day}_DanceScore");
            PlayerPrefs.DeleteKey($"Day{day}_CashierScore");
            PlayerPrefs.DeleteKey($"Day{day}_InfluencerScore");
            PlayerPrefs.DeleteKey($"Day{day}_GentlemenScore");
            PlayerPrefs.DeleteKey($"Day{day}_FriendScore");
            PlayerPrefs.DeleteKey($"Day{day}_DialoguesScore");
            PlayerPrefs.DeleteKey($"Day{day}_OldMan_Spoken");
            PlayerPrefs.DeleteKey($"Day{day}_Kid_Spoken");
            PlayerPrefs.DeleteKey($"Day{day}_NotificationScore");
            PlayerPrefs.DeleteKey($"Day{day}_Notifica1_Score");
            PlayerPrefs.DeleteKey($"Day{day}_Notifica2_Score");
            PlayerPrefs.DeleteKey($"Day{day}_Notifica3_Score");
        }

        PlayerPrefs.Save();
    }
}

