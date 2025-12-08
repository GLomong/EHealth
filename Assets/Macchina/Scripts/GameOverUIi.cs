using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public static bool gameEnded = false;   
    public CarCollision carCollision;     // per leggere i coni colpiti
    public GiocoMan giocoMan;             // per leggere le notifiche swippate

    [Header("Testi punteggio finale")]
    public TMP_Text conesResultText;
    public TMP_Text swipedResultText;
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
    gameEnded = true;

    // aggiorna i testi di punteggio prima di mostrare il pannello
    if (conesResultText != null && carCollision != null)
        conesResultText.text = "Cones hit: " + carCollision.conesHit;

    if (swipedResultText != null && giocoMan != null)
        swipedResultText.text = giocoMan.scoreLabel + giocoMan.score;

    if (gameOverPanel != null)
        gameOverPanel.SetActive(true);

    if (playerCar != null)
        playerCar.canMove = false;

    Time.timeScale = 0f;
}

    public void OnBackToCityPressed()
    {
        // Riabilita il gioco prima di cambiare scena
        gameEnded = false;
        Time.timeScale = 1f;

        // 🔵 PASSO 1: Segna che torni dal minigioco
        PlayerPrefs.SetInt("ReturnFromCarMinigame", 1);

        // 🔵 PASSO 2: Torna alla città
        SceneManager.LoadScene("Città");
    }
}

