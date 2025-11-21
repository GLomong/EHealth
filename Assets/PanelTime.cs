using UnityEngine;

public class PanelTimer : MonoBehaviour
{
    public GameObject panel;   // Il tuo MessagePanel
    public float showDuration = 10f;

    private void Start()
    {
        panel.SetActive(false);   // Sicurezza: parte nascosto
        StartCoroutine(ShowPanelRoutine());
    }

    private System.Collections.IEnumerator ShowPanelRoutine()
    {
        yield return new WaitForSeconds(5f); // attende 1 sec prima di mostrare (modificabile)

        panel.SetActive(true);  // Mostra la vignetta

        yield return new WaitForSeconds(showDuration);

        panel.SetActive(false); // Nasconde la vignetta
    }
}

