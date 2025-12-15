using UnityEngine;
using TMPro;

// Script per gestire tutto il gioco: punteggi, tempo, caduta...
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
        Debug.Log("Penalità applicata! Score ora = " + score);
    }

    public void StopScore()
    {
        scoreActive = false;
    }

    void AggiornaScoreUI()
    {
        scoreText.text = "SCORE: " + score;
    }

    // Calcolo il punteggio finale normalizzato tra 0 e 50, pesando score e time in modo differente (do più importanza al tempo)

    public int CalcolaPunteggioFinale()
    {
        float maxScore = 200f;   //Massimo score che può ottenere in realtà è di più di 200 se cade più volte, però fisso un tetto massimo 
        float maxTime = 40f;     //Fisso anche un tempo massimo in cui l'utente deve completare il gioco che sono 40 secondi

        float scoreNorm = Mathf.Clamp01(score / maxScore);
        float timeNorm = 1f - Mathf.Clamp01(timer / maxTime);

        float weightScore = 0.4f;   //Peso lo score leggermente meno del tempo
        float weightTime = 0.6f;    //Do più importanza al tempo rispetto allo score 

        float combined = (scoreNorm * weightScore) + (timeNorm * weightTime);
        float finalScoreFloat = combined * 50f;

        int finalScoreInt = Mathf.RoundToInt(finalScoreFloat);

        Debug.Log($"[Bridge Final Score] ScoreNorm={scoreNorm}, TimeNorm={timeNorm}, FinalInt={finalScoreInt}");

        return finalScoreInt;
    }

    // ------------------------------ FINE GIOCO ------------------------------

    public void MostraSchermataFinale()
    {
        int finalScore = CalcolaPunteggioFinale();

        // Sulla schermata finale mostro all'utente il punteggio che ha ottenuto e il tempo che ci ha messo
        endGameUI.MostraFineGioco(score, timer);

        // Internamente perà salvo il punteggio finale normalizzato
        SaveScoreForDay(finalScore);
    }

    private void SaveScoreForDay(int finalScore)
    {
        // Salva il punteggio finale del giorno corrente in PlayerPrefs
        int currentDay = TotalGameManager.Instance.CurrentDay;
        finalScore = Mathf.Clamp(finalScore, 0, 50);
        PlayerPrefs.SetInt($"Day{currentDay}_BridgeScore", finalScore);
        PlayerPrefs.Save();

        Debug.Log($"[BridgeGameManager] Salvato Day {currentDay} ScoreFinale = {finalScore}");
    }
    
}

