using System.Collections;
using UnityEngine;

public class TaxiTrigger : MonoBehaviour
{
    public GameObject player;              // Riferimento al ragazzo
    public GameObject fineGiornoPanel;     // Pannello UI "Fine giorno 1"
    public float speed = 5f;               // Velocità taxi

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && !activated)
        {
            activated = true;

            // 1. Nasconde il ragazzo
            player.SetActive(false);

            // 2. Avvia la coroutine che muove il taxi
            StartCoroutine(MoveTaxiOut());
        }
    }

    private IEnumerator MoveTaxiOut()
    {
        // 2. Il taxi si muove verso destra finché non esce
        while (transform.position.x < 15f)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.World);
            yield return null;
        }

        // 3. Mostra pannello "Fine giorno 1"
        fineGiornoPanel.SetActive(true);
    }
}