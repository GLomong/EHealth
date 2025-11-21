using UnityEngine;

public class CartController : MonoBehaviour
{
    public float speed = 5f;
    public float limitX = 4f;

    void Update()
    {
        float input = Input.GetAxis("Horizontal");
        Vector3 pos = transform.position;

        pos.x += input * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, -limitX, limitX);

        transform.position = pos;
    }
}

