using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCity2 : MonoBehaviour
{
    public void LoadMappa2()
    {
        // diciamo alla scena successiva che arrivi dalla disco
        PlayerPrefs.SetInt("ReturnFromDisco", 1);

        SceneManager.LoadScene("Mappa2");
    }
}
