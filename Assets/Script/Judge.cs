using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class Judge : MonoBehaviour
{
    public static Judge Instance;

    [Header("Windows (seconds)")]
    public float perfectWindow = 0.08f;
    public float goodWindow = 0.17f;
    public float lateEarlyWindow = 0.35f;

    [Header("References")]
    public NoteSpawner spawner;
    public ScoreManager scoreManager;

    // manteniamo una lista attiva per ogni lane
    private Dictionary<Lane, List<Note>> activeNotes = new Dictionary<Lane, List<Note>>();

    void Awake()
    {
        Instance = this;
        foreach (Lane l in System.Enum.GetValues(typeof(Lane)))
            activeNotes[l] = new List<Note>();
    }

    public void RegisterActive(Note n)
    {
        activeNotes[n.lane].Add(n);
    }

    public void UnregisterActive(Note n)
    {
        activeNotes[n.lane].Remove(n);
    }

    void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame) TryJudge(Lane.Left);
        if (Keyboard.current.downArrowKey.wasPressedThisFrame) TryJudge(Lane.Down);
        if (Keyboard.current.upArrowKey.wasPressedThisFrame) TryJudge(Lane.Up);
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame) TryJudge(Lane.Right);
    }

    void TryJudge(Lane lane)
    {
        var list = activeNotes[lane];
        if (list == null || list.Count == 0)
        {
            scoreManager.RegisterMiss();
            return;
        }

        Note nearest = list.OrderBy(n => Mathf.Abs((n.spawnTime + n.travelTime) - Time.time)).First();
        float diff = Mathf.Abs((nearest.spawnTime + nearest.travelTime) - Time.time);

        if (diff <= perfectWindow)
        {
            scoreManager.AddScore(100, "Perfect");
            ApplyNoteHit(nearest, "Perfect");
        }
        else if (diff <= goodWindow)
        {
            scoreManager.AddScore(50, "Good");
            ApplyNoteHit(nearest, "Good");
        }
        else if (diff <= lateEarlyWindow)
        {
            scoreManager.AddScore(10, "Late/Early");
            ApplyNoteHit(nearest, "Late");
        }
        else
        {
            scoreManager.RegisterMiss();
        }
    }

    void ApplyNoteHit(Note n, string grade)
    {
        // se è booster, applica effetto
        /* if (n.noteType == NoteType.Booster_Water)
            BoosterManager.Instance.ApplyWater();
        else if (n.noteType == NoteType.Booster_Candy)
            BoosterManager.Instance.ApplyCandy();
        */
        UnregisterActive(n);
        n.MarkJudged();
        // visual feedback: pop-up text, particles ecc (implementa qui)
    }

    // chiamata dal Note quando scade
    public void RegisterMiss(Note n)
    {
        UnregisterActive(n);
        scoreManager.RegisterMiss();
    }
}

