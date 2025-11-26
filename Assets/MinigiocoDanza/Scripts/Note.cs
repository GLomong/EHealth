using UnityEngine;
using System;

public enum Lane { Left, Down, Up, Right }
public enum NoteType { Normal, Booster_Water, Booster_Candy }

public class Note : MonoBehaviour
{
    public Lane lane;
    public NoteType noteType = NoteType.Normal;

    [HideInInspector] public float spawnTime;     // tempo Unity Time.time quando è stato spawnato
    [HideInInspector] public float hitTime;       // tempo in secondi (quando raggiunge il target)
    [HideInInspector] public Vector3 spawnPos;
    [HideInInspector] public Vector3 targetPos;
    [HideInInspector] public float travelTime;    // seconds from spawn -> hit

    public Action<Note> onMiss; // callback

    private bool judged = false;
    public AudioSource musicSource; // passa il riferimento dalla NoteSpawner

    Animator animator;
    public float growDistance = 0.6f; // quando inizia a pulsare

    public bool missed = false;
    public Color missedColor = new Color(1, 1, 1, 0.3f);
    private SpriteRenderer sr;

    public Vector3 defaultScale = new Vector3(1f, 1f, 1f); // scala normale

    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        defaultScale = transform.localScale;
    }

    void Update()
    {
        if (musicSource == null) return;

        if (!missed)
        {
            // Movimento normale 
            float t = (musicSource.time - spawnTime) / travelTime;
            transform.position = Vector3.Lerp(spawnPos, targetPos, t);

            // Controllo vicinanza al target
            float dist = Vector3.Distance(transform.position, targetPos);
            bool near = dist <= growDistance;
            if (animator != null)
            {
                animator.SetBool("NearTarget", near);
            }

            // Controllo se la nota è passata senza essere stata giudicata 
            if (!judged && t >= 1.0f) 
            {
                judged = true;
                missed = true; // segna come mancata
                OnMiss(); // cambia aspetto
            }
        }
        else
        {
            // Continua a scorrere (nota missata)
            Vector3 direction = (targetPos - spawnPos).normalized;
            float speed = Vector3.Distance(spawnPos, targetPos) / travelTime; // velocità costante
            transform.position += direction * speed * Time.deltaTime;

            // Distruggi quando esce dal bordo
            if (Vector3.Distance(transform.position, targetPos) > 5f)
            {
                Destroy(gameObject);
            }
        } 
    }
    public void OnHit()
    {
        judged = true;
        // distrugge subito la nota
        Destroy(gameObject);
    }

    public void OnMiss()
    {
        missed = true;
        if (animator)
        {
            animator.SetBool("NearTarget", false);
            animator.enabled = false;
        }
        // Ripristina la scala originale (evita che rimanga ingrandita)
        transform.localScale = defaultScale;

        if (sr != null)
            sr.color = missedColor; // diventa sbiadita

        // chiama callback miss (chiamato dal Judge)
        onMiss?.Invoke(this);
    }

    // Restituisce il colore del testo del giudizio
    public Color GetColorByGrade(string grade = null)
    {
        // se non viene passato grade, prova a calcolarlo in base al tempo
        if (grade == null)
        {
            float t = (Time.time - spawnTime) / travelTime;
            float dist = Vector3.Distance(transform.position, targetPos);

            if (dist < 0.1f) grade = "Perfect";
            else if (dist < 0.25f) grade = "Good";
            else if (dist < 0.4f) grade = "Late";
            else grade = "Miss";
        }

        switch (grade)
        {
            case "Perfect": return Color.yellow;
            case "Good":    return Color.green;
            case "Late":    return Color.white;
            case "Miss":    return Color.red;
            default:        return Color.white;
        }
    }
}

