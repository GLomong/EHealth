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

    private int score = 0;
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
            // testo iniziale
            scoreText.text = scoreLabel + score;
        }
    }

    void Update()
    {
        if (gameOver) return;

        // countdown
        remainingTime -= Time.deltaTime;
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
        if (gameOver) return;   // se il gioco è finito, non aggiorno più

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

        // blocco la macchina
        if (player != null)
            player.canMove = false;

        // ferma tutto il gioco (strada, coni, notifiche che usano deltaTime)
        Time.timeScale = 0f;

        Debug.Log("FINE GIOCO! Punteggio: " + score);
    }
}
