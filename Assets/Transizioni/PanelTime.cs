using UnityEngine;

public class PanelTimer : MonoBehaviour
{
    public GameObject panel1;   // Primo messaggio
    public GameObject panel2;   // Secondo messaggio
    public float showDuration = 10f; // Durata visibilità di ogni panel

    private void Start()
    {
        // Sicurezza: tenerli nascosti all'inizio
        panel1.SetActive(false);
        panel2.SetActive(false);

        StartCoroutine(ShowPanelsRoutine());
    }

    private System.Collections.IEnumerator ShowPanelsRoutine()
    {
        // ---- MESSAGGIO 1 ----
        yield return new WaitForSeconds(7f); 
        panel1.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        panel1.SetActive(false);

        // Attesa tra i due messaggi
        yield return new WaitForSeconds(10f);

        // ---- MESSAGGIO 2 ----
        panel2.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        panel2.SetActive(false);
    }
}


