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
            if (savedDay > totalDays)
            {
                CurrentDay = 1;
                PlayerPrefs.SetInt(PlayerPrefsCurrentDayKey, 1);
                PlayerPrefs.Save(); 
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
            // Se è fintio il terzo giorno, carica la scena finale
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

    // Esempio di metodo per calcolare la somma totale del gioco
    public int GetTotalGameScore()
    {
        int total = 0;
        for (int day = 1; day <= totalDays; day++)
        {
            // Somma risultati dei vari giorni 
            total += PlayerPrefs.GetInt($"Day{day}_TotalScore", 0);
        }

        return total;
    }

    // Reset generale (utile per test)
    public void ResetAllProgress()
    {
        CurrentDay = 1;
        PlayerPrefs.SetInt(PlayerPrefsCurrentDayKey, 1);

        // Cancella tutti i punteggi
        for (int day = 1; day <= totalDays; day++)
        {
            PlayerPrefs.DeleteKey($"Day{day}_TotalScore");
            PlayerPrefs.DeleteKey($"Day{day}_MarketScore");
            PlayerPrefs.DeleteKey($"Day{day}_CarScore");
            PlayerPrefs.DeleteKey($"Day{day}_BridgeScore");
            PlayerPrefs.DeleteKey($"Day{day}_DanceScore");
        }

        PlayerPrefs.Save();
    }
}

