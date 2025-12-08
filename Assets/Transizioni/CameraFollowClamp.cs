using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraFollowClamp : MonoBehaviour
{
    public Transform target;                // il taxi da seguire
    public TilemapRenderer tilemapRenderer; // TilemapRenderer della tilemap che delimita lo sfondo
    public float smoothTime = 0.15f;

    [HideInInspector]
    public bool activeFollow = false; // se true, la camera segue il target

    Camera cam;
    Vector3 velocity = Vector3.zero;
    float minX, maxX, minY, maxY;
    float halfWidth, halfHeight;

    void UpdateBoundsFromTilemap()
    {
        if (tilemapRenderer != null)
        {
            Bounds b = tilemapRenderer.bounds; // world-space bounds
            minX = b.min.x + halfWidth;
            maxX = b.max.x - halfWidth;
            minY = b.min.y + halfHeight;
            maxY = b.max.y - halfHeight;

            // se il mondo è più piccolo della camera, evita invertire i limiti:
            if (minX > maxX) { float mid = (minX + maxX) / 2f; minX = maxX = mid; }
            if (minY > maxY) { float mid = (minY + maxY) / 2f; minY = maxY = mid; }
        }
    }

    void LateUpdate()
    {
        if (!activeFollow || target == null) return;
        if (cam == null)
        {
            cam = GetComponent<Camera>();
            halfHeight = cam.orthographicSize;
            halfWidth = halfHeight * cam.aspect;
            UpdateBoundsFromTilemap();
        }

        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        // smooth follow
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);

        // clamp ai bordi (la camera si ferma se arriva ai limiti)
        float clampedX = Mathf.Clamp(smoothPos.x, minX, maxX);
        float clampedY = Mathf.Clamp(smoothPos.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    // richiamalo se ridimensioni la tilemap a runtime
    public void RecalculateBounds()
    {
        UpdateBoundsFromTilemap();
    }
}

