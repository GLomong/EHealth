using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitSpriteToCamera : MonoBehaviour
{
    void Start()
    {
        Camera cam = Camera.main;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float height = cam.orthographicSize * 2f;
        float width = height * cam.aspect;

        transform.localScale = new Vector3(
            width / sr.sprite.bounds.size.x,
            height / sr.sprite.bounds.size.y,
            1f
        );
    }
}

