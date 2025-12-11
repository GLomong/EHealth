using UnityEngine;

public class EnterDisco : MonoBehaviour
{
    public string sceneName = "Disco";
    private SceneFade fader;

    private void Start()
    {
        // Trova automaticamente l’oggetto SceneFade nella scena
        fader = FindObjectOfType<SceneFade>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Contollo che non esista già un punteggio del giorno corrente 
        int currentDay = TotalGameManager.Instance.CurrentDay;
        string scoreKey = $"Day{currentDay}_DanceScore";
        if (PlayerPrefs.HasKey(scoreKey))
        {
            Debug.Log($"[EnterDisco] Punteggio per il giorno {currentDay} già esistente: {PlayerPrefs.GetInt(scoreKey)}");
            return;
        }
        if (collision.CompareTag("Player"))
        {
            if (fader != null)
            {
                fader.FadeToScene(sceneName);
            }
            else
            {
                Debug.LogWarning("⚠ Nessun SceneFade trovato nella scena!");
            }
        }
    }
}


