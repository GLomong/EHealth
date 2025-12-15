using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GiocoMan : MonoBehaviour
{
    [Header("Tempo di gioco")]
    public float gameDuration = 60f;  // 1 minuto
    private float remainingTime;

    [Header("UI")]
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public Slider lucidityBar;
    [Tooltip("Testo prima del numero, es: 'Swiped notifications: '")]
    public string scoreLabel = "Swiped notifications: ";
    public int maxLucidity = 20;

    [Header("Riferimenti gioco")]
    public PlayerCar player;
    public GameOverUI gameOverUI;   // collegamento al pannello finale
    public CarCollision carCollision;     // per leggere i coni colpiti

    public int score = 0;
    public bool gameOver { get; private set; } = false;
    public int totalNotificationsSpawned = 0; // per tenere traccia delle notifiche spawnate
    public int totalSpawnedCones = 0; // per tenere traccia dei coni spawnati
    private int finalGradePoints = 0; // punteggio finale calcolato alla fine del gioco
    public int score_coni = 0; // per tenere traccia dei coni colpiti

    void Start()
    {
        remainingTime = gameDuration;

        if (lucidityBar != null)
        {
            lucidityBar.minValue = 0;
            lucidityBar.maxValue = maxLucidity;
            lucidityBar.value = 0;
        }

        if (scoreText != null)
        {
            scoreText.text = scoreLabel + score;
        }
    }

    void Update()
    {

        if (!StartScreen.gameStarted)
            return;

        // se il gioco globale è finito, non faccio più niente
        if (GameOverUI.gameEnded)
            return;

        if (gameOver) return;

        // countdown (uso unscaledDeltaTime così non dipende dal timeScale)
        remainingTime -= Time.unscaledDeltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            EndGame();
        }

        // aggiorna timer MM:SS
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }

    // chiamata quando una notifica viene spostata via
    public void AddLucidity(int amount)
    {
        // se gioco notifiche o gioco globale sono finiti, non aggiungo più punti
        if (gameOver || GameOverUI.gameEnded)
            return;

        score += amount;

        if (lucidityBar != null)
        {
            lucidityBar.value = Mathf.Clamp(score, 0, maxLucidity);
        }

        if (scoreText != null)
        {
            scoreText.text = scoreLabel + score;
        }
    }

    void EndGame()
    {
        if (gameOver) return;
        gameOver = true;

        // blocco la macchina qui solo per sicurezza 
        if (player != null)
            player.canMove = false;

        // fai comparire il pannello di fine gioco UNA SOLA VOLTA
        if (gameOverUI != null && !GameOverUI.gameEnded)
        {
            gameOverUI.ShowGameOver();
        }
        
        // variabili per punteggio
        int finalGradePointsConi = 0;
        int finalGradePointsNotifiche = 0;

        // Calcolo punteggio coni (proporzione con 50 punti massimi)
        if (carCollision != null && totalSpawnedCones > 0)
        {
            score_coni = carCollision.conesHit;
            float scorePercentageConi = Mathf.Clamp01((float)score_coni / (float)totalSpawnedCones); 
            finalGradePointsConi = Mathf.RoundToInt(scorePercentageConi * 50f);
            Debug.Log($"Game over. Punteggio Coni: {finalGradePointsConi} punti su 50");
        }
        else
        {
            finalGradePointsConi = 0; 
        }
        // Calcolo punteggio notifiche (proporzione con 50 punti massimi)
        if (totalNotificationsSpawned > 0)
        {
            float scorePercentage = Mathf.Clamp01((float)score / (float)totalNotificationsSpawned);
            finalGradePointsNotifiche = Mathf.RoundToInt(scorePercentage * 50f);
            Debug.Log($"Game over. Punteggio Notifiche: {finalGradePointsNotifiche} punti su 50");
        }
        else
        {
            finalGradePointsNotifiche = 0;
        }

        // Faccio la media dei due punteggi per il punteggio finale
        finalGradePoints = Mathf.RoundToInt((finalGradePointsConi + finalGradePointsNotifiche) / 2f);

        Debug.Log("FINE GIOCO (notifiche)! Punteggio: " + finalGradePoints + " punti su 50");
        SaveScoreForDay();
    }

    private void SaveScoreForDay()
    {
        // Salva il punteggio finale del giorno corrente in PlayerPrefs
        int currentDay = TotalGameManager.Instance.CurrentDay;
        finalGradePoints = Mathf.Clamp(finalGradePoints, 0, 50);
        PlayerPrefs.SetInt($"Day{currentDay}_CarScore", finalGradePoints);
        PlayerPrefs.Save();
        Debug.Log($"[CarGameManager] Salvato Day {currentDay} Score = {finalGradePoints}");
    }
}
