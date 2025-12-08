using UnityEngine;

public class EnterBridge : MonoBehaviour
{
    private SceneFade sceneFade;

    private void Start()
    {
        sceneFade = FindObjectOfType<SceneFade>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            sceneFade.FadeToScene("Ponte");
        }
    }
}


