using System.Collections.Generic;
using UnityEngine;

public class LaneController : MonoBehaviour
{
    public Transform target;                   // posizione target delle note
    private List<Note> notes = new List<Note>();

    // Chiama questo quando spawni una nota
    public void RegisterNote(Note n)
    {
        notes.Add(n);
        n.targetPos = target.position;
    }

    // Chiama questo quando premi il tasto corrispondente
    public void TryHit()
    {
        if (notes.Count == 0) return;

        Note first = notes[0];
        float dist = Vector3.Distance(first.transform.position, first.targetPos);

        if (dist < 0.1f)
            Debug.Log("Perfect!");
        else if (dist < 0.25f)
            Debug.Log("Good!");
        else if (dist < 0.4f)
            Debug.Log("Late!");
        else
            return;

        first.MarkJudged();   // rimuove la nota
        notes.RemoveAt(0);
    }
}


