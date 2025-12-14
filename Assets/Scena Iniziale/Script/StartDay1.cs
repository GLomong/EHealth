using UnityEngine;
using UnityEngine.SceneManagement;

public class StartDay1 : MonoBehaviour
{
    // Script che permette se si schiaccia il bottone 'StartTheDay' di fare la transizione da questa scena alla scena della città
    public string nextSceneName = "Città"; 

    public void GoToCity()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}