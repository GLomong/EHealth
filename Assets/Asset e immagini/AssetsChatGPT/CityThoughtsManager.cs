using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class CityThoughtsManager : MonoBehaviour
{
    [System.Serializable]
    public class PensieroPerGiorno
    {
        public int giorno;
        public string pensiero;
    }
    public PensieroPerGiorno[] pensieriPerGiorno;
    
    [Header("UI")]
    public TMP_Text pensieroText;
    public CanvasGroup bottomPanel;
    public Button closeButton;  
    
    [Header("Animazioni")]
    public float typeSpeed = 0.05f;

    private bool haMostratoPensieroOggi = false;
    private string chiavePlayerPrefs;
    private bool isTyping = false;

    void Start()
    {
        chiavePlayerPrefs = $"PensieroGiorno_{TotalGameManager.Instance.CurrentDay}_Mostrato";
        
        // Il pannello ci mette 1 secondo per il fade in, quindi aspetto prima di mostrare il pensiero
        StartCoroutine(WaitAndShowThought());
    }

    IEnumerator MostraPensieroDelGiorno()
    {
        bottomPanel.gameObject.SetActive(true);

        // Typewriter effetto
        string pensiero = OttieniPensieroPerGiorno(TotalGameManager.Instance.CurrentDay);
        yield return StartCoroutine(Typewriter(pensiero));

        // Salva dopo typewriter
        PlayerPrefs.SetInt(chiavePlayerPrefs, 1);
        PlayerPrefs.Save();
    }

    IEnumerator Typewriter(string testo)
    {
        isTyping = true;
        pensieroText.text = "";

        foreach (char letter in testo)
        {
            pensieroText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
    }

    // Chiusura manuale come NPC
    public void ClosePensiero()
    {
        if (isTyping)
        {
            StopAllCoroutines();  // Ferma typewriter se cliccano durante
            isTyping = false;
        }
        
        bottomPanel.gameObject.SetActive(false);
    }

    string OttieniPensieroPerGiorno(int giorno)
    {
        foreach (var p in pensieriPerGiorno)
        {
            if (p.giorno == giorno)
                return p.pensiero;
        }
        return "Oggi è una bella giornata...";
    }
    IEnumerator WaitAndShowThought()
    {
        yield return new WaitForSeconds(1f);
        if (PlayerPrefs.GetInt(chiavePlayerPrefs, 0) == 0)
        {
            StartCoroutine(MostraPensieroDelGiorno());
        }
        else
        {
            bottomPanel.gameObject.SetActive(false);
        }
    }
}
