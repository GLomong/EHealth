using UnityEngine;
using TMPro;

//Script per gestire tutto il gioco: punteggi, tempo, caduta...
public class GameManagerBridge : MonoBehaviour
{
    public static GameManagerBridge instance;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    [Header("Score Settings")]
    public int score = 0;
    public int puntiPerAsse = 10;
    public int bonusX2 = 5;
    public int bonusX3 = 10;
    public int penalitaCaduta = 20;

    public float timer = 0f;

    // Inizializzo tempo e score
    private bool timerAttivo = false;
    public bool scoreActive = true;
    public EndGameUI endGameUI;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (timerAttivo)
        {
            timer += Time.deltaTime;
            timeText.text = "TIME: " + timer.ToString("F2");
        }
    }

    // ------------------------------ TIMER ------------------------------

    public void StartTimer()
    {
        timerAttivo = true;
    }

    public void StopTimer()
    {
        timerAttivo = false;
    }

    // ------------------------------ SCORE ------------------------------

    public void AggiungiPunti(int quantiAssi)
    {
        if (!scoreActive) return;

        score += puntiPerAsse;

        if (quantiAssi == 2) score += bonusX2;
        if (quantiAssi == 3) score += bonusX3;

        AggiornaScoreUI();
    }

    public void PenalitaCaduta()
    {
        if (!scoreActive) return;

        score -= penalitaCaduta;
        if (score < 0) score = 0;

        AggiornaScoreUI();
    }

    public void StopScore()
    {
        scoreActive = false;
    }

    void AggiornaScoreUI()
    {
        scoreText.text = "SCORE: " + score;
    }
    public void MostraSchermataFinale()
    {
        endGameUI.MostraFineGioco(score, timer);
        SaveScoreForDay();
    }

    private void SaveScoreForDay()
    {
        // Salva il punteggio finale del giorno corrente in PlayerPrefs
        int currentDay = TotalGameManager.Instance.CurrentDay;
        PlayerPrefs.SetInt($"Day{currentDay}_BridgeScore", score);
        PlayerPrefs.Save();
        Debug.Log($"[BridgeGameManager] Salvato Day {currentDay} Score = {score}");
    }
}

