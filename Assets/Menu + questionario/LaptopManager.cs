using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LaptopManager : MonoBehaviour
{
    [Header("--- PANNELLI ---")]
    public GameObject boxIstruzioni;
    public GameObject gruppoLogin;
    public GameObject pannelloIntroduzione;   // <-- il tuo RecuperoPassword
    public GameObject gruppoDomande;

    [Header("--- INPUT LOGIN ---")]
    public TMP_InputField campoNome;
    public TMP_InputField campoPassword;

    void Start()
    {
        boxIstruzioni.SetActive(true);
        gruppoLogin.SetActive(true);

        pannelloIntroduzione.SetActive(false);  // IMPORTANTE
        gruppoDomande.SetActive(false);         // IMPORTANTE
    }

    // 1️⃣ Bottone "OK" nelle istruzioni
    public void ChiudiIstruzioni()
    {
        boxIstruzioni.SetActive(false);
    }

    // 2️⃣ Bottone "OK" nel login
    public void ConfermaLogin()
    {
        if (campoNome.text != "" && campoPassword.text != "")
        {
            PlayerPrefs.SetString("NomeGiocatore", campoNome.text);

            gruppoLogin.SetActive(false);
            pannelloIntroduzione.SetActive(true);   // MOSTRA INTRO
        }
        else
        {
            Debug.Log("Inserisci nome e password!");
        }
    }

    // 3️⃣ Bottone CONTINUE nell'introduzione
    public void ContinuaAlleDomande()
    {
        pannelloIntroduzione.SetActive(false);
        gruppoDomande.SetActive(true);
    }
}

