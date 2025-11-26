using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;

public class Judge : MonoBehaviour
{
    public static Judge Instance;

    [Header("Windows (seconds)")]
    public float perfectWindow = 0.07f;
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
            // Miss attiva: premuto ma non c'è nessuna nota
            scoreManager.RegisterMiss();
            // togli stamina
            StaminaManager.Instance.ReduceStamina(StaminaManager.Instance.missPenalty);
            // feedback visivo
            HitFeedbackSpawner.Instance.SpawnFeedback("MISS", Color.red, GetLaneTargetPos(lane));
            return;
        }

        float songTime = spawner.musicSource.time;
        
        Note nearest = list.OrderBy(n => Mathf.Abs((n.spawnTime + n.travelTime) - songTime)).First();
        float diff = Mathf.Abs((nearest.spawnTime + nearest.travelTime) - songTime);

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
            // ApplyNoteHit(nearest, "Miss");
        }
    }

    Vector3 GetLaneTargetPos(Lane lane)
    {
        return spawner.targetForLane(lane).position;
    }
    
    void ApplyNoteHit(Note n, string grade)
    {
        // Feedback visivo 
        switch (grade)
        {
            case "Perfect":
                HitFeedbackSpawner.Instance.SpawnFeedback("PERFECT", Color.yellow, n.targetPos);
                break;

            case "Good":
                HitFeedbackSpawner.Instance.SpawnFeedback("GOOD", Color.green, n.targetPos);
                break;

            case "Late":
                HitFeedbackSpawner.Instance.SpawnFeedback("LATE", Color.white, n.targetPos);
                break;

            case "Miss":
                HitFeedbackSpawner.Instance.SpawnFeedback("MISS", Color.red, n.targetPos);
                break;
        }

        UnregisterActive(n);
        n.OnHit();
    }

    // chiamata dal Note quando scade
    public void RegisterMiss(Note n)
    {
        // Miss passiva: nota scaduta
        UnregisterActive(n);

        // Feedback visivo e particelle
        HitFeedbackSpawner.Instance.SpawnFeedback("MISS", Color.red, n.targetPos);
        // HitEffectSpawner.Instance.SpawnEffect("Miss", n.transform.position);
        
        // Riduci stamina
        StaminaManager.Instance.ReduceStamina(StaminaManager.Instance.missPenalty);
        
        scoreManager.RegisterMiss();
    }
}

