using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDay1 : MonoBehaviour
{
    public string nextSceneName = "Città"; // <-- cambia col nome vero della scena città

    public void GoToCity()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}