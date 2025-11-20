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

    void Update()
    {
        float t = (musicSource.time - spawnTime) / travelTime;
        transform.position = Vector3.Lerp(spawnPos, targetPos, t);

        if (!judged && t >= 1.0f) 
        {
            judged = true;
            onMiss?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public void MarkJudged()
    {
        judged = true;
        Destroy(gameObject);
    }
}

