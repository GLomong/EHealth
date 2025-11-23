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
        if (!other.CompareTag("Cart"))
            return;

        CartShake shake = other.GetComponent<CartShake>();

        switch (category)
        {
            case ItemCategory.Essential:
                GameManager.instance.AddScore(+1);
                GameManager.instance.PlayPositive();
                break;

            case ItemCategory.Alcohol:
                GameManager.instance.AddScore(-1);
                GameManager.instance.PlayNegative();
                shake.Shake(0.35f, 12f);
                break;
        }

        Destroy(gameObject);
    }
}



