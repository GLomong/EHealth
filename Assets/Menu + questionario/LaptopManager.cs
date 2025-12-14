using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LaptopManager : MonoBehaviour
{
    [Header("--- PANNELLI ---")]
    public GameObject boxIstruzioni;    //Pannello iniziale di beneveuto e istruzioni per il login successivo
    public GameObject gruppoLogin;      //Login dove inserire nome utente e password
    public GameObject pannelloIntroduzione;   //Pannello tra il login e le domande del questionario dove si dice che le domande successive servono per il recupero della password
    public GameObject gruppoDomande;    //Domande per il recupero della passowrd, tratte dal questinario 

    [Header("--- INPUT LOGIN ---")]
    public TMP_InputField campoNome;
    public TMP_InputField campoPassword;

    void Start()
    {
        boxIstruzioni.SetActive(true);
        gruppoLogin.SetActive(true);

        pannelloIntroduzione.SetActive(false);  //Setto falso perchè all'inizio non si devono vedere
        gruppoDomande.SetActive(false);         
    }

    // Bottone "OK" nelle istruzioni iniziali, premendo ok il box istruzioni si spegne e si apre la schermata login
    public void ChiudiIstruzioni()
    {
        TotalGameManager.Instance.ResetAllProgress(); // Resetta tutti i progressi all'inizio del gioco (compreso questionario e cluster)
        boxIstruzioni.SetActive(false);
    }

    // Bottone "OK" nel login, una volta premuto si apre il pannello di introduzione
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

    // Bottone CONTINUE nell'introduzione: una volta premuto il bottone 'continue' iniziano le domande del questionario:
    public void ContinuaAlleDomande()
    {
        pannelloIntroduzione.SetActive(false);
        gruppoDomande.SetActive(true);
    }
}

