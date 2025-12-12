using UnityEngine;

public class DoorToMarket : MonoBehaviour
{
    public SceneFade sceneFade;          // riferimento al tuo fade panel
    public string sceneToLoad = "Market"; // nome scena

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Contollo che non esista già un punteggio del giorno corrente 
        int currentDay = TotalGameManager.Instance.CurrentDay;
        string scoreKey = $"Day{currentDay}_MarketScore";
        if (PlayerPrefs.HasKey(scoreKey))
        {
            Debug.Log($"[DoorToMarket] Punteggio per il giorno {currentDay} già esistente: {PlayerPrefs.GetInt(scoreKey)}");
            return;
        }
        if (!isTransitioning && other.CompareTag("Player"))
        {
            isTransitioning = true;

            // Avvia la dissolvenza e il cambio scena
            sceneFade.FadeToScene(sceneToLoad);
        }
    }
}