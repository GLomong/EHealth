using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI; 

public class LaptopManager : MonoBehaviour
{
    [Header("--- PANNELLI ---")]
    public GameObject boxIstruzioni;   // Il primo box che appare
    public GameObject gruppoLogin;     // La schermata con nome e password
    public GameObject gruppoDomande;   // La schermata con le 6 domande

    [Header("--- INPUT LOGIN ---")]
    public TMP_InputField campoNome;
    public TMP_InputField campoPassword;

    [Header("--- DOMANDE ---")]
    public ToggleGroup gruppoDomanda1;
    public ToggleGroup gruppoDomanda2;
    public ToggleGroup gruppoDomanda3;
    public ToggleGroup gruppoDomanda4;
    public ToggleGroup gruppoDomanda5;
    public ToggleGroup gruppoDomanda6;

    // Parte all'inizio del gioco
    void Start()
    {
        // Imposto l'ordine iniziale:
        boxIstruzioni.SetActive(true);  // 1. Istruzioni ACCESE
        gruppoLogin.SetActive(true);    // 2. Login ACCESO 
        gruppoDomande.SetActive(false); // 3. Domande SPENTE
    }

    // FUNZIONI PER I BOTTONI 

    // 1. Bottone "OK" nel box iniziale
    public void ChiudiIstruzioni()
    {
        boxIstruzioni.SetActive(false);
    }

    // 2. Bottone "OK" nella schermata Login
    public void ConfermaLogin()
    {
        // Controlla se ha scritto qualcosa
        if (campoNome.text != "" && campoPassword.text != "")
        {
            // Salva il nome per dopo
            PlayerPrefs.SetString("NomeGiocatore", campoNome.text);

            // Cambia schermata: Spegne Login -> Accende Domande
            gruppoLogin.SetActive(false);
            gruppoDomande.SetActive(true);
        }
        else
        {
            Debug.Log("Inserisci nome e password!");
        }
    }

    // 3. Bottone "Confirm Recovery" nella schermata Domande
    public void ConfermaDomande()
    {
        // Controlla se ha messo una crocetta per ogni gruppo
        if (gruppoDomanda1.AnyTogglesOn() && 
            gruppoDomanda2.AnyTogglesOn() && 
            gruppoDomanda3.AnyTogglesOn() && 
            gruppoDomanda4.AnyTogglesOn() && 
            gruppoDomanda5.AnyTogglesOn() && 
            gruppoDomanda6.AnyTogglesOn())
        {
            Debug.Log("Tutte le risposte date! Avvio il gioco...");
            
            // Resetto i giorni 
            TotalGameManager.Instance.ResetAllProgress();
            // CARICA IL LIVELLO SUCCESSIVO
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            Debug.Log("Rispondi a tutte le domande prima di confermare!");
        }
    }

    // Bottone "Cancel" 
    public void PremiCancel()
    {
        campoNome.text = "";
        campoPassword.text = "";
    }
}
