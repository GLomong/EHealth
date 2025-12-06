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
    public GameOverUI gameOverUI;   // aggiunto: collegamento al pannello finale

    public int score = 0;
    public bool gameOver { get; private set; } = false;

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

        // blocco la macchina qui solo per sicurezza (ma lo fa già GameOverUI)
        if (player != null)
            player.canMove = false;

        // fai comparire il pannello di fine gioco UNA SOLA VOLTA
        if (gameOverUI != null && !GameOverUI.gameEnded)
        {
            gameOverUI.ShowGameOver();
        }

        Debug.Log("FINE GIOCO (notifiche)! Punteggio: " + score);
    }
}

