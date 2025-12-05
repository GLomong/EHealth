using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCity : MonoBehaviour


{
    
    public void LoadCity()
    {
        SceneManager.LoadScene("Città");   // Usa esattamente il nome nel Build Settings
    }
}