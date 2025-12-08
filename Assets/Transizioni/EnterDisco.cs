using UnityEngine;

public class EnterDisco : MonoBehaviour
{
    public string sceneName = "Disco";
    private SceneFade fader;

    private void Start()
    {
        // Trova automaticamente l’oggetto SceneFade nella scena
        fader = FindObjectOfType<SceneFade>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (fader != null)
            {
                fader.FadeToScene(sceneName);
            }
            else
            {
                Debug.LogWarning("⚠ Nessun SceneFade trovato nella scena!");
            }
        }
    }
}


