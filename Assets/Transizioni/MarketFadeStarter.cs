using UnityEngine;

//Dissolvenza per entrare al market
public class MarketFadeStarter : MonoBehaviour
{
    public SceneFade sceneFade;     // riferimento al FadePanel
    public GameObject fadePanelObj; // GameObject del pannello di fade

    void Start()
    {
        // Avvia il fade-in
        sceneFade.FadeIn();

        // Disattiva il pannello dopo il tempo di fade
        Invoke(nameof(DisableFadePanel), sceneFade.fadeDuration);
    }

    void DisableFadePanel()
    {
        fadePanelObj.SetActive(false);
    }
}