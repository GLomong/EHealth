using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    public GameObject panelFine;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;

    public void MostraFineGioco(int score, float time)
    {
        panelFine.SetActive(true);

        finalScoreText.text = "SCORE: " + score;
        finalTimeText.text = "TIME: " + time.ToString("F2");
    }

    public void VaiAllaProssimaScena(string nomeScena)
    {
        // diciamo alla scena successiva che arrivi dalla fine gioco
        PlayerPrefs.SetInt("ReturnFromBridge", 1);

        SceneManager.LoadScene(nomeScena);
    }
}
