using UnityEngine;

public class DanceGameManager : MonoBehaviour
{
    public float gameDuration = 60f; // 60 secondi di gioco, è public quindi è modificabile dall'Inspector
                                     // 90 mi sembrava troppo lungo
    private float timer;
    public AudioSource musicSource; 
    public ScoreManager scoreManager;
    public NoteSpawner noteSpawner;
    public int finalGradePoints = 0; //punteggio finale su max 50 

    private bool gameActive = true;

    void Start()
    {
        timer = gameDuration;
        if (musicSource != null)
            musicSource.Play();
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
        gameActive = false;

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
            int finalScore = scoreManager.score;
            int totalNotes = noteSpawner.totalNotesSpawned;
            int maxScore = totalNotes * 5; // punteggio per il perfect = 5 punti per nota
            float scorePercentage = (float)finalScore / (float)maxScore; 
            finalGradePoints = Mathf.RoundToInt(scorePercentage * 50);
            Debug.Log($"Game over. Punteggio Finale: {finalGradePoints} punti su 50");
        }

        // Debug.Log("GAME OVER — Tempo scaduto");

        // Qui possisamo mettere un pannello di fine partita
        // UIManager.Instance.ShowGameOver();
        // qua mostrerei il punteggio che compare già a schermo durante il gioco
    }
}

