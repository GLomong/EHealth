using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int score = 0;
    public int combo = 0;
    public int maxCombo = 0;

    public void AddScore(int amount, string grade)
    {
        combo++;
        score += amount * (1 + combo/10); // semplice moltiplicatore per combo
        if (combo > maxCombo) maxCombo = combo;
        // UI update qui
        Debug.Log($"Hit: {grade} +{amount}  Score: {score} Combo: {combo}");
    }

    public void RegisterMiss()
    {
        combo = 0;
        Debug.Log("Miss!");
        // UI update qui
    }
}

