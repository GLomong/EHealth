using UnityEngine;

public class BridgeTrigger : MonoBehaviour
{
    public SceneFade fadeScript;
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            fadeScript.FadeToScene(nextSceneName);
            PlayerPrefs.SetString("SpawnPoint", "FromBridge");

        }
    }
}

