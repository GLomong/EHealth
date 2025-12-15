using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    [Header("Prefabs & Transforms")]
    public GameObject notePrefab;
    public Transform spawnerLeft, spawnerDown, spawnerUp, spawnerRight;
    public Transform targetLeft, targetDown, targetUp, targetRight;
    public Sprite spriteLeft;
    public Sprite spriteDown;
    public Sprite spriteUp;
    public Sprite spriteRight;
    public bool canSpawn = true;
    public int totalNotesSpawned = 0;

    [Header("Timing")]
    public float travelTime = 2.0f;            // tempo che la nota impiega dallo spawn al target
    public AudioSource musicSource;            // AudioSource della canzone
    public float autoSpawnInterval = 1.0f;     // intervallo tra spawn automatici

    private float nextAutoSpawnTime = 0f;      // tempo della canzone per il prossimo spawn

    void Start()
    {
        // ricavo il cluster dell'utente 
        int cluster = PlayerPrefs.GetInt("ClusterUtente", 1);

        // regolo l'autoSpawnInterval in base al cluster (cluster salvati da 1 a 4)
        switch (cluster)
        {
            case 1: // LUCA (droga 8/10)
                // note più veloci e spawn più frequenti
                travelTime = 1.5f;
                autoSpawnInterval = 0.7f;
                break;
            case 2: // PIETRO (droga 4/10)
                // note leggermente piu veloci del normale ma spawn normale 
                travelTime = 1.7f;
                autoSpawnInterval = 1.0f;
                break;
            case 3: // ELENA (droga 0/10) 
                // note normali e spawn normali
                travelTime = 2.0f;
                autoSpawnInterval = 1.0f;
                break;
            case 4: // FRANCESCO (droga 2/10)
                // note normali ma spawn leggermente più frequenti
                travelTime = 2.0f;
                autoSpawnInterval = 0.8f;
                break;
        }
        Debug.Log($"Cluster {cluster} → travelTime={travelTime}, spawnInterval={autoSpawnInterval}");

        // incremento la difficoltà in base al giorno
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        float difficultyIncrement = 0.05f * (currentDay - 1); // primo giorno = 0, secondo = 0.05, terzo = 0.10
        autoSpawnInterval = Mathf.Max(0.2f, autoSpawnInterval - difficultyIncrement);
        travelTime = Mathf.Max(0.5f, travelTime - difficultyIncrement);
        Debug.Log($"Giorno {currentDay} → autoSpawnInterval={autoSpawnInterval}");
    }

    void Update()
    {
        if (!canSpawn) return;
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
        totalNotesSpawned++;
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

        SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
        // Imposta lo sprite in base alla lane
        switch(lane)
        {
            case Lane.Left:  sr.sprite = spriteLeft; break;
            case Lane.Down:  sr.sprite = spriteDown; break;
            case Lane.Up:    sr.sprite = spriteUp; break;
            case Lane.Right: sr.sprite = spriteRight; break;
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

   public  Transform targetForLane(Lane l)
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
