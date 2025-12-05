using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public static bool gameEnded = false;   

    public GameObject gameOverPanel;
    public PlayerCar playerCar;

    void Start()
    {
        // assicura che il gioco parta sempre sbloccato
        Time.timeScale = 1f;

        gameEnded = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        // segnale globale che il gioco è finito
        gameEnded = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // blocca la macchina
        if (playerCar != null)
            playerCar.canMove = false;

        // blocca tutto il gioco (animazioni, spawn, movimento, timer)
        Time.timeScale = 0f;
    }

    public void OnBackToCityPressed()
    {
        // Riabilita il gioco prima di cambiare scena
        gameEnded = false;
        Time.timeScale = 1f;

        // cambia il nome con la tua scena vera
        SceneManager.LoadScene("CityScene");
    }
}

