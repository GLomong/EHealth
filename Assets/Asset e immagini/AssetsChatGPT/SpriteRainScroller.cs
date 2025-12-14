using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRainScroller : MonoBehaviour
{
    public Vector2 scrollSpeed = new Vector2(0f, -1.5f);

    SpriteRenderer sr;
    Material mat;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        mat = sr.material; // istanza materiale
    }

    void Update()
    {
        Vector2 offset = mat.mainTextureOffset;
        offset += scrollSpeed * Time.deltaTime;
        offset.x -= Mathf.Floor(offset.x);
        offset.y -= Mathf.Floor(offset.y);
        mat.mainTextureOffset = offset;
    }
}

