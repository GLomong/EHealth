using UnityEngine;
using UnityEngine.SceneManagement; // Serve per cambiare scena

public class MainMenuManager : MonoBehaviour
{
    // Questa funzione verrà chiamata dal bottone
    public void StartGame()
    {
        // Carica la scena numero 1 (la prossima in lista)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
