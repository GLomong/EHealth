using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    public float timeRemaining = 60f;     // durata della partita
    public TMP_Text timerText;            // riferimento al testo
    public GameOverUI gameOverUI;         // riferimento a GameOverUI

    void Update()
    {
        // Se il gioco è finito, NON aggiornare il timer
        if (GameOverUI.gameEnded)
            return;

        // scala il tempo
        timeRemaining -= Time.unscaledDeltaTime;  

        if (timeRemaining < 0f)
            timeRemaining = 0f;

        // aggiorna il testo
        if (timerText != null)
        {
            int seconds = Mathf.CeilToInt(timeRemaining);
            timerText.text = seconds.ToString();
        }

        // quando finisce il tempo
        if (timeRemaining <= 0f)
        {
            if (!GameOverUI.gameEnded) // per evitare doppie chiamate
            {
                if (gameOverUI != null)
                    gameOverUI.ShowGameOver();
            }
        }
    }
}