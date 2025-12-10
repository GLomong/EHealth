 using UnityEngine;
 using TMPro;
 using UnityEngine.UI;
 using UnityEngine.SceneManagement;
 
 [System.Serializable]
 public class Domanda
 {
     public string testo;
     public string[] risposte;   // 
 }
 
 public class QuestionController : MonoBehaviour
 {
     //Inizializzo un array di domande dove inserirò il numero di domande e per ogni domanda inserisco il testo. Per ogni domanda si apre una freccetta in cui inserire testo, numero di risposte e testo delle risposte. 
     public Domanda[] domande;
    //Nell'incpector dovrò inserire il testo che ho creato nel canvas della hierarchy per far capire dove dovrà essere posizionato il testo di ogni domanda nella scena
     public TMP_Text textDomanda;
 
     // Array di testi per le risposte: qui inserirò nell'inspector i testi delle risposte (sarebbero i label dei toggle: TextA, TextB, TextC, ...)
     public TMP_Text[] textRisposte;
 
     // Array di toggle: inserisco nell'inspector i toggle (ToggleA, ToggleB, ToggleC, ...)
     public Toggle[] toggles;
 
     private int index = 0;
 
     void Start()
     {
         MostraDomanda();
     }
 
     public void Next()
     {
         // Ogni volta che premo 'next' compare la domanda successiva
         index++;
 
         // Se finsice il questionario, quindi quando l'indice della domanda è uguale al numero della domanda allora se premo 'next' non parte un'altra domanda
         // ma la scena successiva che in questo caso è 'InizioGiornata1' 
         if (index >= domande.Length)
         {
             SceneManager.LoadScene("InizioGiornata1");
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
 }
