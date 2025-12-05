using UnityEngine;
using System.Collections;

public class CarShake : MonoBehaviour
{
    public void Shake(float duration, float angle)
    {
        StartCoroutine(ShakeRoutine(duration, angle));
    }

    IEnumerator ShakeRoutine(float duration, float angle)
    {
        Quaternion startRot = transform.rotation;

        float t = 0f;
        while (t < duration)
        {
            float rot = Mathf.Sin(t * 50f) * angle;
            transform.rotation = Quaternion.Euler(0, 0, rot);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = startRot;
    }
}



