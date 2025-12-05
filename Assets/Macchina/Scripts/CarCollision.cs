using UnityEngine;
using TMPro;   

public class CarCollision : MonoBehaviour
{
    private CarShake carShake;

    [Header("Shake")]
    public float shakeDuration = 0.2f;
    public float shakeAngle = 5f;

    [Header("Coni colpiti")]
    public int conesHit = 0;
    public TMP_Text conesText;   

    void Awake()
    {
        carShake = GetComponent<CarShake>();
        UpdateUI();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cono"))
        {
            // tremolio macchina
            if (carShake != null)
                carShake.Shake(shakeDuration, shakeAngle);

            // incrementa punteggio
            conesHit++;
            UpdateUI();

            // butta via il cono 
            ConeFall cone = other.GetComponent<ConeFall>();
            if (cone != null)
            {
                float dir = Mathf.Sign(other.transform.position.x - transform.position.x);
                if (dir == 0f) dir = 1f;
                cone.Fall(dir);
            }

            Debug.Log("Cono colpito, totale = " + conesHit);
        }
    }

    void UpdateUI()
    {
        if (conesText != null)
        {
            conesText.text = "Cones hit: " + conesHit;
        }
    }
}