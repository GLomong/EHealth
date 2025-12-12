using UnityEngine;

//Script per far cadere gli oggetti dall'alto con diverse probabilità:
public class ObjectSpawner : MonoBehaviour
{
    //Definisco dei campi pubblici dove inserire gli oggetti essenziali (quanti e quali) e gli oggetti alcolici (quanti e quali)
    public GameObject[] essentialItems;
    public GameObject[] alcoholItems;

    //Definisco ogni quanto voglio che scenda un oggetto
    public float spawnInterval = 0.8f;
    public float minX = -3.5f;
    public float maxX = 3.5f;

    //Qui posso definire la probabilità con cui voglio che scenda un oggetto essenziale o un oggetto alcool
    [Header("Probabilità dinamica alcol")]
    [Range(0f, 1f)]
    public float alcoholStartProbability = 0.10f;  // probabilità iniziale (10%)
    [Range(0f, 1f)]
    public float alcoholEndProbability = 0.40f;    // probabilità finale (40%)

    float timer;
    
    void Start()
    {
        // ricavo il cluster dell'utente 
        int cluster = PlayerPrefs.GetInt("UserCluster", 1);

        // regolo lo spawnInterval in base al cluster (cluster salvati da 1 a 4)
        switch (cluster)
        {
            case 1: // LUCA (alcol 3/40)
                // spawn oggetti normale 
                spawnInterval = 0.8f;
                alcoholStartProbability = 0.10f;
                alcoholEndProbability = 0.30f;
                break;
            case 2: // PIETRO (alcol 10/40)
                // spawn oggetti velocizzato
                spawnInterval = 1.0f;
                alcoholStartProbability = 0.10f;
                alcoholEndProbability = 0.40f;
                break;
            case 3: // FRANCESCO (alcol 2/40)
                // spawn oggetti normale
                spawnInterval = 0.8f;
                alcoholStartProbability = 0.10f;
                alcoholEndProbability = 0.30f;
                break;
            case 4: // ELENA (alcol 26.5/40)
                // spawn oggetti velocizzato
                spawnInterval = 1.2f;
                alcoholStartProbability = 0.30f;
                alcoholEndProbability = 0.60f;
                break;
        }
        Debug.Log($"Cluster {cluster} → alcoholStartProbability={alcoholStartProbability}, alcoholEndProbability={alcoholEndProbability}, spawnInterval={spawnInterval}");

        // incremento la difficoltà in base al giorno
        int currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        float difficultyIncrement = 0.05f * (currentDay - 1); // primo giorno = 0, secondo = 0.05, terzo = 0.10
        spawnInterval = Mathf.Max(0.2f, spawnInterval - difficultyIncrement);
        alcoholEndProbability = Mathf.Min(1.0f, alcoholEndProbability + difficultyIncrement);
        Debug.Log($"Giorno {currentDay} → spawnInterval={spawnInterval}, alcoholEndProbability={alcoholEndProbability}");
    }

    void Update()
    {
        if (!GameManager.instance.isGameStarted)
            return;

        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnItem();
            timer = 0f;
        }
    }

    void SpawnItem()
    {
        GameObject prefab;

        // Calcolo la probabilità basata sul tempo di gioco
        float t = Mathf.Clamp01(Time.time / GameManager.instance.gameDuration);
        float alcoholProbability = Mathf.Lerp(alcoholStartProbability, alcoholEndProbability, t);

        // Debug: controlla che funzioni
        // Debug.Log("Probabilità attuale: " + alcoholProbability);

        // Uso la probabilità dinamica
        if (Random.value < alcoholProbability)
        {
            // Spawn ALCOOL
            prefab = alcoholItems[Random.Range(0, alcoholItems.Length)];
        }
        else
        {
            // Spawn ESSENZIALI
            prefab = essentialItems[Random.Range(0, essentialItems.Length)];
        }

        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, transform.position.y, 0);

        Instantiate(prefab, pos, Quaternion.identity);
    }
}