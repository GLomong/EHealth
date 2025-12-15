using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Script per gestire tutto il gioco: score, tempo, barra dello score, audio, pannello istruzioni e pannello finale...
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
    public AudioClip positiveSfx;       //Audio positivo
    public AudioClip negativeSfx;       //Audio negativo

    [Header("Game Timer")]
    public float gameDuration = 40f;                 // durata totale del gioco
    public GameObject gameOverPanel;                 // pannello finale
    public TMP_Text finalScoreText;                  // score finale da mostrare

    [Header("Start Game UI")]
    public GameObject instructionPanel;              // pannello iniziale con START
    public bool isGameStarted = false;               // blocca tutto finché non parte

    private AudioSource audioSource;                 //Audio quando prendi gli oggetti
    private bool gameEnded = false;


    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateUI();

        // Il gioco parte fermo
        Time.timeScale = 0f;

        if (instructionPanel != null)
            instructionPanel.SetActive(true);

        // Nascondo il pannello finale all'inizio
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Faccio iniziare il gioco quando dal pannello istruzioni schiaccio 'start', a quel punto il pannello istruzioni scompare e il gioco si attiva:
    public void StartGame()
    {
        isGameStarted = true;

        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        Time.timeScale = 1f;  // Parte il tempo -> a scalare

        StartCoroutine(GameTimer());
    }

    //Aggiorno il punteggio
    public void AddScore(int amount)
    {
        if (gameEnded || !isGameStarted) return;

        score += amount;
        score = Mathf.Clamp(score, 0, maxScore);
        UpdateUI();
    }

    //Aggiorno la barra in base al punteggio
    void UpdateUI()
    {
        scoreFillBar.fillAmount = (float)score / maxScore;
        scoreText.text = score.ToString();
    }

    public void PlayPositive() =>
        audioSource.PlayOneShot(positiveSfx);

    public void PlayNegative() =>
        audioSource.PlayOneShot(negativeSfx);


    // Il gioco finisce quando finisce il tempo
    private System.Collections.IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(gameDuration);
        EndGame();
    }

    // a questo punto blocco il gioco e mostro il pannello con lo score finale e il bottone per tornare alla città
    private void EndGame()
    {
        if (gameEnded) return;
        gameEnded = true;

        Time.timeScale = 0f;  // Fermo il gioco

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
        int safesScore = Mathf.Clamp(score, 0, 50);
        PlayerPrefs.SetInt($"Day{currentDay}_MarketScore", safesScore);
        PlayerPrefs.Save();
        Debug.Log($"[MarketGameManager] Salvato Day {currentDay} Score = {safesScore}");
    }
}

