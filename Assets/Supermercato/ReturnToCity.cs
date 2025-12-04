using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCity : MonoBehaviour
{
    [Header("Panel che copre tutto")]
    public GameObject fadePanel; // ← da assegnare nell’inspector

    public void LoadCity()
    {
        if (fadePanel != null)
            fadePanel.SetActive(false);   // 🔥 disattiva il pannello nero

        SceneManager.LoadScene("Città", LoadSceneMode.Single);  // 🔥 carica la scena pulita
    }
}