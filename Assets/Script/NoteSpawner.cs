using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs & Transforms")]
    public GameObject notePrefab;
    public Transform spawnerLeft, spawnerDown, spawnerUp, spawnerRight;
    public Transform targetLeft, targetDown, targetUp, targetRight;

    [Header("Timing")]
    public float travelTime = 2.0f;            // tempo che la nota impiega dallo spawn al target
    public AudioSource musicSource;            // AudioSource della canzone
    public float autoSpawnInterval = 1.5f;     // intervallo tra spawn automatici

    private float nextAutoSpawnTime = 0f;      // tempo della canzone per il prossimo spawn

    void Update()
    {
        if (!musicSource) return;

        float songTime = musicSource.time;

        // spawn automatico basato sul tempo della canzone
        if (songTime >= nextAutoSpawnTime)
        {
            SpawnRandomNote(songTime);
            nextAutoSpawnTime = songTime + autoSpawnInterval;
        }
    }

    void SpawnRandomNote(float songTime)
    {
        Lane lane = (Lane)Random.Range(0, 4); // lane casuale

        // crea entry temporanea per spawn
        BeatEntry entry = new BeatEntry
        {
            lane = lane,
            hitTime = songTime + travelTime,
            noteType = NoteType.Normal
        };

        Transform spawnT = spawnerForLane(lane);
        Transform targetT = targetForLane(lane);

        GameObject go = Instantiate(notePrefab, spawnT.position, Quaternion.identity, transform);
        Note note = go.GetComponent<Note>();
        note.lane = lane;
        note.noteType = entry.noteType;
        note.spawnTime = songTime;
        note.hitTime = entry.hitTime;
        note.spawnPos = spawnT.position;
        note.targetPos = targetT.position;
        note.travelTime = travelTime;
        note.musicSource = musicSource;
        note.onMiss = (n) => { Judge.Instance.RegisterMiss(n); };

        Judge.Instance.RegisterActive(note);

        // Ruota la freccia in base alla lane
        switch(lane)
        {
            case Lane.Left:  go.transform.rotation = Quaternion.Euler(0,0,90); break;
            case Lane.Down:  go.transform.rotation = Quaternion.Euler(0,0,180); break;
            case Lane.Up:    go.transform.rotation = Quaternion.Euler(0,0,0); break;
            case Lane.Right: go.transform.rotation = Quaternion.Euler(0,0,-90); break;
        }
    }

    Transform spawnerForLane(Lane l)
    {
        switch (l)
        {
            case Lane.Left: return spawnerLeft;
            case Lane.Down: return spawnerDown;
            case Lane.Up: return spawnerUp;
            default: return spawnerRight;
        }
    }

    Transform targetForLane(Lane l)
    {
        switch (l)
        {
            case Lane.Left: return targetLeft;
            case Lane.Down: return targetDown;
            case Lane.Up: return targetUp;
            default: return targetRight;
        }
    }

    [System.Serializable]
    public class BeatEntry
    {
        public Lane lane;
        public float hitTime;
        public NoteType noteType = NoteType.Normal;
    }
}
