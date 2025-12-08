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
    public GameObject attentionUIPrefab;   // <-- TENIAMO SOLO ATTENTION
    public float attentionDuration = 2f;

    [Header("Suono")]
    public AudioClip suonoRottura;

    [Header("Uscita di scena")]
    public Transform puntoUscita;
    public float speedUscita = 3f;
    private bool uscitaIniziata = false;

    void Start()
    {
        StartCoroutine(EntrataInScena());
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

    // ---------------------------------------------------------
    // ENTRATA
    // ---------------------------------------------------------
    IEnumerator EntrataInScena()
    {
        targetPos = punti[0].position;
        staMuovendo = true;

        while (staMuovendo)
            yield return null;

        ingressoFinito = true;

        GameManagerBridge.instance.StartTimer();
    }

    // ---------------------------------------------------------
    // AVANZA
    // ---------------------------------------------------------
    public void Avanza(int quantiAssi)
    {
        if (!ingressoFinito || staMuovendo || inCooldown) return;

        GameManagerBridge.instance.AggiungiPunti(quantiAssi);

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

        indexCorrente += quantiAssi;
        if (indexCorrente >= punti.Length)
            indexCorrente = punti.Length - 1;

        // USCITA FINALE
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

    // ---------------------------------------------------------
    // CADUTA (Senza Asse Rotta)
    // ---------------------------------------------------------
    IEnumerator Caduta()
    {
        Debug.Log("💀 CADUTO!");

        GameManagerBridge.instance.PenalitaCaduta();

        // ATTENTION UI
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

        // SUONO
        if (suonoRottura != null)
            AudioSource.PlayClipAtPoint(suonoRottura, Camera.main.transform.position, 1f);

        // ANIMAZIONE CADUTA
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

    // ---------------------------------------------------------
    // USCITA FINALE
    // ---------------------------------------------------------
    IEnumerator UscitaFinale()
    {
        if (uscitaIniziata) yield break;
        uscitaIniziata = true;

        Debug.Log("🏁 Fine ponte → uscita!");

        //GameObject.Find("ButtonsManager").SetActive(false);

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

        //Debug.Log("🎉 Personaggio uscito!");
    }
}



