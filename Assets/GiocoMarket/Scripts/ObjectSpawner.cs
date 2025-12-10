using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] essentialItems;
    public GameObject[] alcoholItems;

    public float spawnInterval = 0.8f;
    public float minX = -3.5f;
    public float maxX = 3.5f;

    [Header("Probabilità dinamica alcol")]
    [Range(0f, 1f)]
    public float alcoholStartProbability = 0.10f;  // probabilità iniziale (10%)
    [Range(0f, 1f)]
    public float alcoholEndProbability = 0.40f;    // probabilità finale (40%)

    float timer;

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

        // Calcola probabilità basata sul tempo di gioco
        float t = Mathf.Clamp01(Time.time / GameManager.instance.gameDuration);
        float alcoholProbability = Mathf.Lerp(alcoholStartProbability, alcoholEndProbability, t);

        // Debug: controlla che funzioni
        // Debug.Log("Probabilità attuale: " + alcoholProbability);

        // Usa la probabilità dinamica
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