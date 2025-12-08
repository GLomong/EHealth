using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 60f;     // durata della partita in secondi
    public TMP_Text timerText;           // testo che mostra il tempo 
    public GameOverUI gameOverUI;        // riferimento al pannello di fine gioco

    void Start()
    {
        // mostra subito il valore iniziale (60)
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = seconds.ToString();
        }
    }

    void Update()
    {
        // finché non ho premuto START, il timer è fermo
        if (!StartScreen.gameStarted)
            return;

        // se il gioco è finito, non faccio più nulla
        if (GameOverUI.gameEnded)
            return;

        // countdown
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f)
            timeRemaining = 0f;

        // aggiorna il testo
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = seconds.ToString();
        }

        // quando arriva a zero → mostra pannello finale
        if (timeRemaining <= 0f)
        {
            if (gameOverUI != null && !GameOverUI.gameEnded)
            {
                gameOverUI.ShowGameOver();
            }
        }
    }
}
