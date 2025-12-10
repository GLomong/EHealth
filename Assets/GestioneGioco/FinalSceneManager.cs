using UnityEngine;
using TMPro;

public class FinalSceneManager : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    void Start()
    {
        int totalScore = TotalGameManager.Instance.GetTotalGameScore();
        finalScoreText.text = "Total Score: " + totalScore;
        Debug.Log("Punteggio finale totale: " + totalScore);
    }
}
