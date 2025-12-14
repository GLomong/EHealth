using UnityEngine;
using UnityEngine.SceneManagement;

//Script per tornare dal market alla città
public class ReturnToCity : MonoBehaviour
{
    public void LoadCity()
    {
        // Dico alla scena successiva che arriviamo dal Market
        PlayerPrefs.SetInt("ReturnFromMarket", 1);
        SceneManager.LoadScene("Città");
    }
}