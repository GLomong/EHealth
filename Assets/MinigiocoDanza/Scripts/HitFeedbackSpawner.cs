using UnityEngine;
using TMPro;

public class HitFeedbackSpawner : MonoBehaviour
{
    public static HitFeedbackSpawner Instance;

    public GameObject feedbackPrefab;
    public Transform canvasParent;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnFeedback(string text, Color color, Vector3 worldPos)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        GameObject fb = Instantiate(feedbackPrefab, canvasParent);
        fb.transform.position = screenPos;

        TextMeshProUGUI tmp = fb.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.color = color;
    }
}

