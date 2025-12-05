using UnityEngine;

public class StartScreen : MonoBehaviour
{
    public GameObject startPanel;   // pannello iniziale con le istruzioni
    public PlayerCar playerCar;     // riferimento alla macchina

    void Start()
    {
        // Assicuriamo che il gioco parta SEMPRE sbloccato
        GameOverUI.gameEnded = false;
        Time.timeScale = 0f;

        // mostra pannello iniziale
        if (startPanel != null)
            startPanel.SetActive(true);

        // blocca la macchina finché non premi START
        if (playerCar != null)
            playerCar.canMove = false;
    }

    // chiamata dal bottone START
    public void OnStartButtonPressed()
    {
        // nasconde la schermata iniziale
        if (startPanel != null)
            startPanel.SetActive(false);

        // ora la macchina può muoversi
        if (playerCar != null)
            playerCar.canMove = true;

        // riavvia il tempo
        Time.timeScale = 1f;

        // Sicurezza extra:
        GameOverUI.gameEnded = false;
    }
}

