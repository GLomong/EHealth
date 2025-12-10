using UnityEngine;

public class GroceryItem : MonoBehaviour
{
    public enum ItemCategory
    {
        Essential,
        Alcohol
    }

    public ItemCategory category;
    public float fallSpeed = 3f;

    void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < -6f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ================================
        // 1) SE TOCCA LA ZONA ALTA (VALIDA)
        // ================================
        if (other.CompareTag("PickupZone"))
        {
            CartShake shake = other.GetComponentInParent<CartShake>();

            switch (category)
            {
                case ItemCategory.Essential:
                    GameManager.instance.AddScore(+1);
                    GameManager.instance.PlayPositive();
                    break;

                case ItemCategory.Alcohol:
                    GameManager.instance.AddScore(-1);
                    GameManager.instance.PlayNegative();
                    if (shake != null)
                        shake.Shake(0.35f, 12f);
                    break;
            }

            Destroy(gameObject);
            return;
        }

        // ===================================
        // 2) SE TOCCA LA ZONA BASSA (IGNORE)
        // ===================================
        if (other.CompareTag("IgnoreZone"))
        {
            // NON fare niente!
            // L'oggetto continua a cadere normalmente.
            return;
        }
    }
}




