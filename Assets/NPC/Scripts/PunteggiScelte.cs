using UnityEngine;

public class PunteggiScelte : MonoBehaviour
{
    public GameObject ChoicePanel;
    
    public void ChoiceButtonClick(int punteggio)
    {
        // Aggiungo il punteggio -> riferimento a punteggi del gioco
        ChoicePanel.SetActive(false);
        
    }
}
