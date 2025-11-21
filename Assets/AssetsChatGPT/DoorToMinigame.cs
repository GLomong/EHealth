using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorToMinigame : MonoBehaviour
{
    [Header("Scene del minigioco")]
    public string miniGameSceneName = "Market";

    [Header("UI")]
    public GameObject pressKeyUI;      // il testo "Premi E per entrare"
    public KeyCode interactionKey = KeyCode.E;

    private bool playerInRange = false;

    private void Start()
    {
        if (pressKeyUI != null)
            pressKeyUI.SetActive(false);   // il testo parte nascosto
    }

    private void Update()
    {
        // Se il player è davanti alla porta e preme E
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {
            SceneManager.LoadScene(miniGameSceneName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;

            if (pressKeyUI != null)
                pressKeyUI.SetActive(true);   // mostra "Premi E"
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;

            if (pressKeyUI != null)
                pressKeyUI.SetActive(false);  // nasconde il testo
        }
    }
}