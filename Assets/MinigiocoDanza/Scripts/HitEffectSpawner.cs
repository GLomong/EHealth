using UnityEngine;

public class HitEffectSpawner : MonoBehaviour
{
    public static HitEffectSpawner Instance;

    public GameObject perfectEffect;
    public GameObject goodEffect;
    public GameObject lateEffect;
    public GameObject missEffect;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnEffect(string grade, Vector3 worldPos)
    {
        GameObject prefab = null;

        switch (grade)
        {
            case "Perfect": prefab = perfectEffect; break;
            case "Good":    prefab = goodEffect; break;
            case "Late":    prefab = lateEffect; break;
            case "Miss":    prefab = missEffect; break;
        }

        if (prefab != null)
        {
            GameObject go = Instantiate(prefab, worldPos, Quaternion.identity);
            Destroy(go, 1f); // si autodistrugge
        }
    }
}

