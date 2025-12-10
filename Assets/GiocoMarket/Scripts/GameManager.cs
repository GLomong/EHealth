using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Score Settings")]
    public int score = 0;
    public int maxScore = 50;

    [Header("UI")]
    public Image scoreFillBar;
    public TMP_Text scoreText;

    [Header("SFX")]
    public AudioClip positiveSfx;
    public AudioClip negativeSfx;

    [Header("Game Timer")]
    public float gameDuration = 40f;                 // durata totale del gioco
    public GameObject gameOverPanel;                 // pannello finale
    public TMP_Text finalScoreText;                  // score finale da mostrare

    [Header("Start Game UI")]
    public GameObject instructionPanel;              // pannello iniziale con START
    public bool isGameStarted = false;               // blocca tutto finché non parte

    private AudioSource audioSource;
    private bool gameEnded = false;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateUI();

        // FERMA IL GIOCO ALL'INIZIO
        Time.timeScale = 0f;

        if (instructionPanel != null)
            instructionPanel.SetActive(true);

        // Nascondi il pannello finale all'inizio
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // ===========================================
    //                START GAME
    // ===========================================
    public void StartGame()
    {
        isGameStarted = true;

        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        Time.timeScale = 1f;  // RIATTIVA IL GIOCO

        StartCoroutine(GameTimer());
    }

    public void AddScore(int amount)
    {
        if (gameEnded || !isGameStarted) return;

        score += amount;
        score = Mathf.Clamp(score, 0, maxScore);
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreFillBar.fillAmount = (float)score / maxScore;
        scoreText.text = score.ToString();
    }

    public void PlayPositive() =>
        audioSource.PlayOneShot(positiveSfx);

    public void PlayNegative() =>
        audioSource.PlayOneShot(negativeSfx);


    // ===========================================
    //                GAME TIMER
    // ===========================================
    private System.Collections.IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration);
        EndGame();
    }

    // ===========================================
    //                GAME OVER
    // ===========================================
    private void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 0f;  // FERMA IL GIOCO

        if (finalScoreText != null)
            finalScoreText.text = "\n" + score.ToString();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        
        SaveScoreForDay();
    }
    
    private void SaveScoreForDay()
    {
        // Salva il punteggio finale del giorno corrente in PlayerPrefs
        int currentDay = TotalGameManager.Instance.CurrentDay;
        PlayerPrefs.SetInt($"Day{currentDay}_MarketScore", score);
        PlayerPrefs.Save();
        Debug.Log($"[MarketGameManager] Salvato Day {currentDay} Score = {score}");
    }
}

