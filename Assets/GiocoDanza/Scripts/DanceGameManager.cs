using UnityEngine;

public class DanceGameManager : MonoBehaviour
{
    public float gameDuration = 60f; // 60 secondi di gioco, è public quindi è modificabile dall'Inspector
                                     // 90 mi sembrava troppo lungo
    private float timer;
    public AudioSource musicSource; 
    public ScoreManager scoreManager;
    public NoteSpawner noteSpawner;
    public int finalScore = 0; //punteggio finale grezzo
    public int finalGradePoints = 0; //punteggio finale su max 50 
    public GameObject gameOverPanel; // pannello finale
    public TMPro.TextMeshProUGUI finalScoreText;        

    private bool gameActive = true;

    void Start()
    {
        timer = gameDuration;
        if (musicSource != null)
            musicSource.Play();

        // Nascondi il pannello finale all'inizio
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        if (!gameActive) return;
        gameActive = false;

        Time.timeScale = 0f;  // FERMA IL GIOCO

        // Stop musica
        if (musicSource != null)
            musicSource.Stop();

        // Stop allo spawner
        NoteSpawner spawner = FindObjectOfType<NoteSpawner>();
        if (spawner != null)
            spawner.canSpawn = false;
        
        // Calcolo punteggio finale (proporzione con 50 punti massimi)
        if (scoreManager != null)
        {
            finalScore = scoreManager.score;
            int totalNotes = noteSpawner.totalNotesSpawned;
            int maxScore = totalNotes * 5; // punteggio per il perfect = 5 punti per nota
            float scorePercentage = (float)finalScore / (float)maxScore; 
            finalGradePoints = Mathf.RoundToInt(scorePercentage * 50);
            // Debug.Log($"Game over. Punteggio Finale: {finalGradePoints} punti su 50");
        }

        if (finalScoreText != null)
            finalScoreText.text = "\n" + finalScore.ToString();     // mostra il punteggio finale (quello che appare sullo schermo, non convertito in 50 punti massimo)

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        SaveScoreForDay();
    }

    private void SaveScoreForDay()
    {
        // Salva il punteggio finale del giorno corrente in PlayerPrefs
        int currentDay = TotalGameManager.Instance.CurrentDay;
        PlayerPrefs.SetInt($"Day{currentDay}_DanceScore", finalGradePoints);
        PlayerPrefs.Save();
        Debug.Log($"[DanceGameManager] Salvato Day {currentDay} Score = {finalGradePoints}");
    }
}

