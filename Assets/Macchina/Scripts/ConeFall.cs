using UnityEngine;
using System.Collections;

public class ConeFall : MonoBehaviour
{
    public float duration = 0.4f;      // quanto ci mette a uscire
    public float destroyDelay = 0.5f;  // dopo quanto lo distrugge

    private bool isFalling = false;

    // direction: +1 = verso destra, -1 = verso sinistra
    public void Fall(float direction)
    {
        if (isFalling) return;
        StartCoroutine(FallRoutine(direction));
    }

    private IEnumerator FallRoutine(float direction)
    {
        isFalling = true;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("Nessuna Camera con tag MainCamera trovata!");
            yield break;
        }

        Vector3 startPos = transform.position;

        // posizione del cono in coordinate viewport (0-1)
        Vector3 viewportPos = cam.WorldToViewportPoint(startPos);

        // lo spingiamo fuori dallo schermo
        float targetViewportX = (direction > 0) ? 1.2f : -0.2f; // un po' oltre il bordo
        viewportPos.x = targetViewportX;

        // un filo più in basso, ma resta coerente con l'altezza attuale
        viewportPos.y = Mathf.Clamp01(viewportPos.y - 0.2f);

        Vector3 endPos = cam.ViewportToWorldPoint(viewportPos);

        // disattivo il collider così la macchina non ci sbatte più
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        float t = 0f;
        while (t < duration)
        {
            float lerpT = t / duration;
            transform.position = Vector3.Lerp(startPos, endPos, lerpT);
            t += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        // aspetto un attimo e poi lo distruggo
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
