using UnityEngine;

public class MovimentoPonte : MonoBehaviour
{
    public Transform[] punti;   // I waypoint delle assi del ponte
    public float speed = 4f;    // Velocità di movimento

    private int indexCorrente = 0;  // Da quale asse parte

    private bool staMuovendo = false;
    private Vector3 targetPos;

    void Update()
    {
        if(staMuovendo)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                speed * Time.deltaTime
            );

            // Se è arrivato al punto
            if(Vector3.Distance(transform.position, targetPos) < 0.05f)
                staMuovendo = false;
        }
    }

    public void Avanza(int quantiAssi)
    {
        if (staMuovendo) return; // Evita doppio input durante il movimento

        indexCorrente += quantiAssi;

        // Impedisci di superare la lunghezza del ponte
        if (indexCorrente >= punti.Length)
            indexCorrente = punti.Length - 1;

        targetPos = punti[indexCorrente].position;
        staMuovendo = true;
    }
}