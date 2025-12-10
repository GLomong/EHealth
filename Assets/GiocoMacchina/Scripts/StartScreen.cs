using UnityEngine;

public class StartScreen : MonoBehaviour
{
 
    public static bool gameStarted = false;

    public GameObject startPanel;
    public PlayerCar playerCar;

    void Start()
    {
        GameOverUI.gameEnded = false;
        gameStarted = false;

        if (startPanel != null)
            startPanel.SetActive(true);

        if (playerCar != null)
            playerCar.canMove = false;

        Time.timeScale = 0f; // blocca tutto
    }

    public void OnStartButtonPressed()
    {
        if (startPanel != null)
            startPanel.SetActive(false);

        if (playerCar != null)
            playerCar.canMove = true;

        gameStarted = true;
        Time.timeScale = 1f;
    }
}
