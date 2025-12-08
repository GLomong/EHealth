using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCity : MonoBehaviour
{
    public void LoadCity()
    {
        // diciamo alla scena successiva che arrivi dal Market
        PlayerPrefs.SetInt("ReturnFromMarket", 1);

        SceneManager.LoadScene("Città");
    }
}