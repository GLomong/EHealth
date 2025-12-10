using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // <-- AGGIUNGI QUESTO

public class QuestionController : MonoBehaviour
{
    public string[] domande;

    public string[] risposteA;
    public string[] risposteB;
    public string[] risposteC;
    public string[] risposteD;
    public string[] risposteE;
    public string[] risposteF;

    public TMP_Text textDomanda;
    public TMP_Text textA;
    public TMP_Text textB;
    public TMP_Text textC;
    public TMP_Text textD;
    public TMP_Text textE;
    public TMP_Text textF;


    public Toggle toggleA;
    public Toggle toggleB;
    public Toggle toggleC;
    public Toggle toggleD;
    public Toggle toggleE;
    public Toggle toggleF;

    private int index = 0;

    void Start()
    {
        MostraDomanda();
    }

    public void Next()
    {
        index++;

        // 🔥 QUANDO FINISCE IL QUESTIONARIO → CARICA LA NUOVA SCENA
        if (index >= domande.Length)
        {
            SceneManager.LoadScene("InizioGiornata1"); 
            return;
        }

        MostraDomanda();
    }

    void MostraDomanda()
    {
        textDomanda.text = domande[index];
        textA.text = risposteA[index];
        textB.text = risposteB[index];
        textC.text = risposteC[index];
        textD.text = risposteD[index];
        textE.text = risposteE[index];
        textF.text = risposteF[index];

        toggleA.isOn = false;
        toggleB.isOn = false;
        toggleC.isOn = false;
        toggleD.isOn = false;
        toggleE.isOn = false;
        toggleF.isOn = false;
    }

}