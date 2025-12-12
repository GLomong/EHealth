using UnityEngine;
using System.Collections;

public class MovimentoPonte : MonoBehaviour
{
    public Transform[] punti;

    [Header("Velocità")]
    public float speed = 4f;
    public float speedEntrata = 2f;
    public float speedPassoSingolo = 2.5f;

    [Header("Probabilità di cadere")]
    [Range(0, 1)] public float probCadutaX2 = 0.2f;
    [Range(0, 1)] public float probCadutaX3 = 0.4f;

    [Header("Cooldown X1")]
    public float attesaPassoSingolo = 1f;

    private int indexCorrente = 0;
    private bool staMuovendo = false;
    private bool ingressoFinito = false;
    private bool inCooldown = false;

    private Vector3 targetPos;

    [Header("UI Caduta")]
    public GameObject attentionUIPrefab;
    public float attentionDuration = 2f;

    [Header("Suono")]
    public AudioClip suonoRottura;

    [Header("Uscita di scena")]
    public Transform puntoUscita;
    public float speedUscita = 3f;
    private bool uscitaIniziata = false;

    
    [Header("Avvio Manuale")]
    public bool avvioConsentito = false;


    void Start()
    {
       // Siccome ho inserito il pannello istruzioni non faccio partire la scena subito ppea schiaccio play ma aspetto che venga schiacciato il bottone 'start'
        // StartCoroutine(EntrataInScena());
        
    }

    void Update()
    {
        if (staMuovendo)
        {
            float vel = ingressoFinito ? speed : speedEntrata;

            if (ingressoFinito && speed == speedPassoSingolo)
                vel = speedPassoSingolo;

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                vel * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPos) < 0.05f)
                staMuovendo = false;
        }
    }

    // Funzione per far avviare il gioco solo quando viene schiacciato il bottone 'start' del pannello istruzioni
    public void AvviaGioco()
    {
        if (!avvioConsentito)
        {
            avvioConsentito = true;
            StartCoroutine(EntrataInScena());
        }
    }

    //Quando schiaccio start il pannello scompare, c'è l'entrata automatica del personaggio alla scena del ponte e parte poi timer punteggi e i tasti vengono
    //abilitati per giocare
    IEnumerator EntrataInScena()
    {
        targetPos = punti[0].position;
        staMuovendo = true;

        while (staMuovendo)
            yield return null;

        ingressoFinito = true;

        GameManagerBridge.instance.StartTimer();
    }

    // Faccio avanzare il personaggio in base a quante assi sceglie di saltare alla volta:
    public void Avanza(int quantiAssi)
    {
        if (!ingressoFinito || staMuovendo || inCooldown) return;
        
        // Prima controllo se cade, se non cade aggiungo punti, se cade tolgo la penalità caduta

        if (quantiAssi == 1)
        {
            speed = speedPassoSingolo;
            StartCoroutine(CooldownPassoSingolo());
        }
        else
        {
            speed = 4f;
        }

        if (quantiAssi == 2 && Random.value < probCadutaX2)
        {
            StartCoroutine(Caduta());
            return;
        }

        if (quantiAssi == 3 && Random.value < probCadutaX3)
        {
            StartCoroutine(Caduta());
            return;
        }
        
        GameManagerBridge.instance.AggiungiPunti(quantiAssi);
        
        indexCorrente += quantiAssi;
        if (indexCorrente >= punti.Length)
            indexCorrente = punti.Length - 1;

        if (indexCorrente == punti.Length - 1)
        {
            StartCoroutine(UscitaFinale());
            return;
        }

        targetPos = punti[indexCorrente].position;
        staMuovendo = true;
    }

    IEnumerator CooldownPassoSingolo()
    {
        inCooldown = true;
        yield return new WaitForSeconds(attesaPassoSingolo);
        inCooldown = false;
    }

    // Se il personaggio cade esce una scritta in alto con 'Oh no, try again'
    IEnumerator Caduta()
    {
        Debug.Log("💀 CADUTO!");
        Debug.Log("ScoreActive = " + GameManagerBridge.instance.scoreActive + " | Score prima = " + GameManagerBridge.instance.score);

        GameManagerBridge.instance.PenalitaCaduta();

        if (attentionUIPrefab != null)
        {
            GameObject uiAtt = Instantiate(attentionUIPrefab, GameObject.Find("Canvas").transform);

            RectTransform rt = uiAtt.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;

            Destroy(uiAtt, attentionDuration);
        }

        if (suonoRottura != null)
            AudioSource.PlayClipAtPoint(suonoRottura, Camera.main.transform.position, 1f);

        Vector3 cadutaPos = transform.position + new Vector3(0, -1f, 0);

        float t = 0;
        while (t < 1f)
        {
            transform.position = Vector3.Lerp(transform.position, cadutaPos, t);
            t += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.4f);

        indexCorrente = 0;
        transform.position = punti[0].position;

        staMuovendo = false;
    }

    // Quando il personaggio raggiunge il punto di uscita, automaticamente esce di scena e compare la schermata finale con time e score finali dell'utente.
    IEnumerator UscitaFinale()
    {
        if (uscitaIniziata) yield break;
        uscitaIniziata = true;

        Debug.Log("🏁 Fine ponte → uscita!");

        GameManagerBridge.instance.StopTimer();
        GameManagerBridge.instance.StopScore();

        staMuovendo = true;
        targetPos = puntoUscita.position;

        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speedUscita * Time.deltaTime
            );

            yield return null;
        }

        staMuovendo = false;

        GameManagerBridge.instance.MostraSchermataFinale();
    }
}


