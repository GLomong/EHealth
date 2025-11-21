using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] essentialItems;
    public GameObject[] alcoholItems;

    public float spawnInterval = 0.8f;
    public float minX = -3.5f;
    public float maxX = 3.5f;

    float timer;

    void Update()
    {
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

        int category = Random.Range(0, 2); // 0 = essential, 1 = alcohol

        if (category == 0)
            prefab = essentialItems[Random.Range(0, essentialItems.Length)];
        else
            prefab = alcoholItems[Random.Range(0, alcoholItems.Length)];

        float x = Random.Range(minX, maxX);
        Vector3 pos = new Vector3(x, transform.position.y, 0);

        Instantiate(prefab, pos, Quaternion.identity);
    }
}

