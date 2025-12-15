using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Domanda
{
    public string testo; // testo della domanda
    public string[] risposte;   // risposte 
    public int[] punteggi;  // punteggi associati a ogni risposta
}

[System.Serializable]
public class RispostaUtente
{
    public int domandaIndex;
    public int rispostaIndex;
    public string rispostaTesto;
    public int punteggio;
}

public class QuestionController : MonoBehaviour
{
    [Header("Questionario")]
    //Inizializzo un array di domande dove inserirò il numero di domande e per ogni domanda inserisco il testo. Per ogni domanda si apre una freccetta in cui inserire testo, numero di risposte e testo delle risposte. 
    public Domanda[] domande;
//Nell'incpector dovrò inserire il testo che ho creato nel canvas della hierarchy per far capire dove dovrà essere posizionato il testo di ogni domanda nella scena
    public TMP_Text textDomanda;

    // Array di testi per le risposte: qui inserirò nell'inspector i testi delle risposte (sarebbero i label dei toggle: TextA, TextB, TextC, ...)
    public TMP_Text[] textRisposte;

    // Array di toggle: inserisco nell'inspector i toggle (ToggleA, ToggleB, ToggleC, ...)
    public Toggle[] toggles;

    [Header("Transizioni")]
    public SceneFade fadeCanvas;

    private int index = 0;
    private List<RispostaUtente> risposteDate = new List<RispostaUtente>();

    // CENTROIDI DEI 4 CLUSTER PREDEFINITI: alcol, droghe, gioco, internet
    private readonly float[][] centroidi = new float[4][]
    {
        new float[] { 0.075f, 0.8f, 0f, 0.645f }, // Cluster 1: LUCA
        new float[] { 0.25f, 0.4f, 0.074f, 0.16f }, // Cluster 2: PIETRO
        new float[] { 0.6625f, 0f, 0.5556f, 0.39f },  // Cluster 3: ELENA
        new float[] { 0.05f, 0.2f, 0.222f, 0.91f } // Cluster 4: FRANCESCO
    };

    void Start()
    {
        MostraDomanda();
    }

    public void Next()
    {
        // Controllo che una risposta sia stata selezionata 
        if (!RispostaSelezionata())
        {
            Debug.Log("Seleziona una risposta prima di procedere.");
            return;
        }

        // Valuto la risposta data dall'utente
        ValutaRisposta();

        // Ogni volta che premo 'next' compare la domanda successiva
        index++;

        // Se finsice il questionario, quindi quando l'indice della domanda è uguale al numero della domanda allora se premo 'next' non parte un'altra domanda
        // ma la scena successiva che in questo caso è 'InizioGiornata1' 
        if (index >= domande.Length)
        {
            // Calcolo punteggio per coppie di domande [alcol, droga, gioco, internet]
            float[] featureUtente = CalcolaSommeCoppie();
            
            // Assegno cluster in base alla distanza dai centroidi
            int clusterAssegnato = AssignCluster(featureUtente);
            Debug.Log($"Cluster assegnato: {clusterAssegnato + 1}");

            // Salvataggio 
            PlayerPrefs.SetInt("ClusterUtente", clusterAssegnato + 1); // +1 per avere cluster da 1 a 4
            PlayerPrefs.Save();

            //SceneManager.LoadScene("InizioGiornata1");
            fadeCanvas.FadeToScene("InizioGiornata1");
            return;
        }

        MostraDomanda();
    }

    void MostraDomanda()
    {
        Domanda d = domande[index];

        // Testo domanda: per ogni domanda scrivo il testo nell'inspector nella zona giusta
        textDomanda.text = d.testo;

        // Nascondo tutti i toggle e le risposte all'inizio, perchè non tutte le domande hanno 6 risposte, alcune per esempio ne hanno solo 2, quindi all'inizio le nascondo tutte
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].gameObject.SetActive(false);
            textRisposte[i].gameObject.SetActive(false);
            toggles[i].isOn = false;
        }

        // e poi attivo solo i toggle e le risppste che servono in base alla domanda --> gestisco tutto nell'inspector.
        for (int i = 0; i < d.risposte.Length; i++)
        {
            toggles[i].gameObject.SetActive(true);
            textRisposte[i].gameObject.SetActive(true);

            textRisposte[i].text = d.risposte[i];
        }
    }

    void ValutaRisposta()
    {
        Domanda d = domande[index];

        // Cerco il toggle attivo 
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                // indice della risposta data dall'utente
                int rispostaIndex = i;
                int punteggioRisposta = d.punteggi[rispostaIndex];

                // Salvo la risposta nella lista
                var risposta = new RispostaUtente
                {
                    domandaIndex = index,
                    rispostaIndex = rispostaIndex,
                    rispostaTesto = d.risposte[rispostaIndex],
                    punteggio = punteggioRisposta
                };
                risposteDate.Add(risposta);

                break; // Esco dal loop
            }
        }
    }
    
    bool RispostaSelezionata()
    {
        foreach (var t in toggles)
            if (t.isOn)
                return true;

        return false;
    }

    float[] CalcolaSommeCoppie()
    {
        float[] sommeCoppie = new float[4]; // alcol, droghe, gioco, internet

        for (int i = 0; i < 4; i++)
        {
            int idx1 = i*2;
            int idx2 = idx1 + 1;

            // Punteggi dati dall'utente per le due domande della coppia
            int punteggio1 = risposteDate[idx1].punteggio;
            int punteggio2 = (idx2 < risposteDate.Count) ? risposteDate[idx2].punteggio : 0;

            float sommaUtente = punteggio1 + punteggio2;

            // Calcolo il massimo punteggio possibile per la coppia
            Domanda domanda1 = domande[idx1];
            int max1 = 0;
            if (domanda1 != null)
            {
                foreach (int p in domanda1.punteggi)
                {
                    if (p > max1) max1 = p;
                }
            }
            Domanda domanda2 = (idx2 < domande.Length) ? domande[idx2] : null;
            int max2 = 0;
            if (domanda2 != null)
            {
                foreach (int p in domanda2.punteggi)
                {
                    if (p > max2) max2 = p;
                }
            }
            
            float sommaMassima = max1 + max2;

            // Normalizzo il punteggio tra 0 e 1
            float punteggioNorm = (sommaMassima > 0) ? (
                sommaUtente / sommaMassima) : 0f;

            sommeCoppie[i] = punteggioNorm;
        }

        return sommeCoppie;
    }
    
    // Assegno il cluster più vicino al vettore dell'utente 
    int AssignCluster(float[] featureUtente)
    {
        int clusterAssegnato = -1;
        float distanzaMinima = float.MaxValue;

        for (int i = 0; i < centroidi.Length; i++)
        {
            float distanza = CalcolaDistanzaEuclidea(featureUtente, centroidi[i]);
            if (distanza < distanzaMinima)
            {
                distanzaMinima = distanza;
                clusterAssegnato = i;
            }
        }

        return clusterAssegnato;
    }

    // Distanza euclidea tra due vettori
    float CalcolaDistanzaEuclidea(float[] a, float[] b)
    {
        float somma = 0f;
        for (int i = 0; i < a.Length; i++)
        {
            float diff = a[i] - b[i];
            somma += diff * diff;
        }

        return Mathf.Sqrt(somma);
    }
}