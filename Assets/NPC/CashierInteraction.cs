using UnityEngine;

public class CashierInteraction : MonoBehaviour, IInteractable
{
    // Questo dice se il Cashier può essere interagito (per ora sempre sì)
    public bool CanInteract()
    {
        return true;
    }

    // Questa funzione viene chiamata quando il giocatore preme E mentre è vicino
    public void Interact()
    {
        Debug.Log("Interazione con il Cassiere!");
        // Qui in futuro farai partire il dialogo
    }
}

