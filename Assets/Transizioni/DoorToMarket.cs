using UnityEngine;

public class DoorToMarket : MonoBehaviour
{
    public SceneFade sceneFade;          // riferimento al tuo fade panel
    public string sceneToLoad = "Market"; // nome scena

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTransitioning && other.CompareTag("Player"))
        {
            isTransitioning = true;

            // Avvia la dissolvenza e il cambio scena
            sceneFade.FadeToScene(sceneToLoad);
        }
    }
}