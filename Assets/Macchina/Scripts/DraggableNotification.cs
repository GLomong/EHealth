using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableNotification : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 startPosition;
    public float dismissDistance = 250f;   // quanto lontano devo trascinarla
    public PlayerCar player;               // trascino qui il PlayerCar

    void Awake() 
    {
        rectTransform = GetComponent<RectTransform>(); //recupera il recttransf della notifica
        startPosition = rectTransform.anchoredPosition; //salvo posizione initale notifica
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition; //salva la posizione di partenza (rimanda indietro se non abbastanza fuori)
    }

    public void OnDrag(PointerEventData eventData) //la notifica si sposta seguendo dito
    {
        // canvas overlay: delta ok così
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float distanceFromCenter = rectTransform.anchoredPosition.magnitude; //quanto ho spostato la notifica dal centro

        if (distanceFromCenter > dismissDistance)
        {
            //buttata via abbastanza lontano -> sparisce
            gameObject.SetActive(false);

            if (player != null)
                player.canMove = true;   // ridò il controllo alla macchina
        }
        else
        {
            // non trascinata abbastanza -> torna al centro
            rectTransform.anchoredPosition = startPosition;
        }
    }
}
