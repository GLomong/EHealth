using UnityEngine;
using TMPro; // serve per TextMeshPro UI
using System.Collections; // per lampeggio testo

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int combo = 0;
    public int maxCombo = 0;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI scoreTotalText;
    public float textVisibleDuration = 0.5f; // quanto tempo rimane visibile il messaggio
    public float popScale = 1.2f;       // grandezza del pop
    public float popDuration = 0.15f;   // durata del pop
    public float fadeDuration = 0.25f; // durata del fade out
    
    private Coroutine displayCoroutine;
    private Vector3 originalScale;
    private Color originalColor;

    void Start()
    {
        originalScale = scoreText.transform.localScale;
        originalColor = scoreText.color;
        scoreText.gameObject.SetActive(false); // testo nascosto all’inizio
        UpdateScoreUI();
    }
    public void AddScore(int amount, string grade)
    {
        combo++;
        score += amount * (1 + combo/10); // semplice moltiplicatore per combo
        if (combo > maxCombo) maxCombo = combo;

        UpdateScoreUI(); // aggiorna l'UI ogni volta che cambia lo score

        if (grade == "Perfect")
            ShowScoreText($"<color=yellow>{grade}: +{amount}!</color>\nCombo: {combo}");
        else if (grade == "Good")
            ShowScoreText($"<color=green>{grade}: +{amount}!</color>\nCombo: {combo}");
        else
            ShowScoreText($"{grade}: +{amount}!\nCombo: {combo}");
    }
    public void RegisterMiss()
    {
        combo = 0;
        UpdateScoreUI(); // aggiorna l'UI ogni volta che cambia lo score
        ShowScoreText($"<color=red>Miss: +0\nStamina -10</color>");
    }
    void UpdateScoreUI()
    {
        if (scoreTotalText != null)
        {
            scoreTotalText.text = $"Score: {score}";
        }
    }
    private void ShowScoreText(string text)
    {
        // ferma eventuale coroutine in corso
        if (displayCoroutine != null)
            StopCoroutine(displayCoroutine);

        scoreText.text = text;
        displayCoroutine = StartCoroutine(DisplayCoroutine());
    }
    private IEnumerator DisplayCoroutine()
    {
        scoreText.gameObject.SetActive(true);    // mostra testo

        // Reset per sicurezza
        scoreText.transform.localScale = originalScale;
        scoreText.color = originalColor;

        // FASE 1: Pop-in scale animation
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime / popDuration;
            float scale = Mathf.Lerp(1f, popScale, t);
            scoreText.transform.localScale = originalScale * scale;
            yield return null;
        }

        // torna alla scala normale
        scoreText.transform.localScale = originalScale;

        // FASE 2: Rimane visibile
        yield return new WaitForSeconds(textVisibleDuration);

        // FASE 3: Fade out
        t = 0;
        Color startColor = scoreText.color;
        while (t < 1)
        {
            t += Time.deltaTime / fadeDuration;
            startColor.a = Mathf.Lerp(1f, 0f, t);
            scoreText.color = startColor;
            yield return null;
        }

        // FASE 4: Nascondi completamente
        scoreText.gameObject.SetActive(false);
    }
}

