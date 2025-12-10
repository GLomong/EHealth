using UnityEngine;

//Script per far comparire le due tentazioni durante il gioco del market
public class PanelTimer : MonoBehaviour
{
    public GameObject panel1;   // Primo messaggio
    public GameObject panel2;   // Secondo messaggio
    public float showDuration = 10f; // Durata visibilità di ogni panel 10 secondi

    private void Start()
    {
        // All'inizio li nascondo entrambi 
        panel1.SetActive(false);
        panel2.SetActive(false);

        StartCoroutine(ShowPanelsRoutine());
    }

    private System.Collections.IEnumerator ShowPanelsRoutine()
    {
        // ---- Tentazione 1 ----
        yield return new WaitForSeconds(7f); 
        panel1.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        panel1.SetActive(false);

        // Attesa tra i due messaggi di 10 secondi
        yield return new WaitForSeconds(10f);

        // ---- Tentazione 2 ----
        panel2.SetActive(true);
        yield return new WaitForSeconds(showDuration);
        panel2.SetActive(false);
    }
}


