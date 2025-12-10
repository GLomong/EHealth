 using UnityEngine;
 using TMPro;
 using UnityEngine.UI;
 using UnityEngine.SceneManagement;
 
 [System.Serializable]
 public class Domanda
 {
     public string testo;
     public string[] risposte;   // Numero variabile: 2, 3, 4, 5, 6…
 }
 
 public class QuestionController : MonoBehaviour
 {
     public Domanda[] domande;
 
     public TMP_Text textDomanda;
 
     // Array di testi per le risposte (TextA, TextB, TextC, ...)
     public TMP_Text[] textRisposte;
 
     // Array di toggle (ToggleA, ToggleB, ToggleC, ...)
     public Toggle[] toggles;
 
     private int index = 0;
 
     void Start()
     {
         MostraDomanda();
     }
 
     public void Next()
     {
         index++;
 
         // SE FINISCE IL QUESTIONARIO → VAI ALLA SCENA SUCCESSIVA
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
 
         // TESTO DOMANDA
         textDomanda.text = d.testo;
 
         // 1️⃣ NASCONDI TUTTI I TOGGLE E TESTI
         for (int i = 0; i < toggles.Length; i++)
         {
             toggles[i].gameObject.SetActive(false);
             textRisposte[i].gameObject.SetActive(false);
             toggles[i].isOn = false;
         }
 
         // 2️⃣ ATTIVA SOLO QUELLI CHE SERVONO
         for (int i = 0; i < d.risposte.Length; i++)
         {
             toggles[i].gameObject.SetActive(true);
             textRisposte[i].gameObject.SetActive(true);
 
             textRisposte[i].text = d.risposte[i];
         }
     }
 }
