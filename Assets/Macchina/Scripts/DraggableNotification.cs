using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableNotification : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Vector2 startPosition;

    [Header("Impostazioni notifica")]
    public float dismissDistance = 250f;   // quanto la si deve trascinare per buttarla via

    [Header("Riferimenti")]
    public PlayerCar player;   
    public GiocoMan giocoMan;  

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        
        rectTransform.localScale = new Vector3(1.2f, 1.2f, 1f);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // se il gioco è finito, non si può iniziare a trascinare
        if (GameOverUI.gameEnded)
            return;

        // salvo la posizione di partenza
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // se il gioco è finito, niente drag
        if (GameOverUI.gameEnded)
            return;

        // sposto la notifica seguendo il dito/mouse
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // se il gioco è finito, ignoro il rilascio
        if (GameOverUI.gameEnded)
        {
            // giusto per sicurezza, la riporto dov’era
            rectTransform.anchoredPosition = startPosition;
            return;
        }

        // distanza dal centro (0,0) del canvas
        float distanceFromCenter = rectTransform.anchoredPosition.magnitude;

        if (distanceFromCenter > dismissDistance)
        {
            // trascinata abbastanza lontano -> la notifica scompare
            gameObject.SetActive(false);

            // ridò il controllo alla macchina
            if (player != null)
                player.canMove = true;

            // aggiungo 1 punto di lucidità
            if (giocoMan != null)
                giocoMan.AddLucidity(1);
        }
        else
        {
            // non spostata abbastanza -> torna alla posizione iniziale
            rectTransform.anchoredPosition = startPosition;
        }
    }
}
