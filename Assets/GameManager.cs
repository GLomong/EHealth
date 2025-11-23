using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public int maxScore = 50;

    public Image scoreFillBar;
    public TMP_Text scoreText;

    public AudioClip positiveSfx;
    public AudioClip negativeSfx;

    AudioSource audioSource;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        score = Mathf.Clamp(score, 0, maxScore);
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreFillBar.fillAmount = (float)score / maxScore;
        scoreText.text = score.ToString();
    }

    public void PlayPositive() =>
        audioSource.PlayOneShot(positiveSfx);

    public void PlayNegative() =>
        audioSource.PlayOneShot(negativeSfx);
}

