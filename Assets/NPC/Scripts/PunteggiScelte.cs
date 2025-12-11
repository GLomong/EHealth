using UnityEngine;

public class PunteggiScelte : MonoBehaviour
{
    public GameObject ChoicePanel;

    public string npcName;
    
    public void ChoiceButtonClick(int punteggio)
    {
        if (npcName == "Cashier")
        {
            // Salva il punteggio del dialogo con NPC Cashier (Market)
            int currentDay = TotalGameManager.Instance.CurrentDay;
            PlayerPrefs.SetInt($"Day{currentDay}_CashierScore", punteggio);
            PlayerPrefs.Save();
            Debug.Log($"[NPC_Cashier] Salvato Day {currentDay} Score = {punteggio}");
        }
        else if (npcName == "Influencer")
        {
            // Salva il punteggio del dialogo con NPC Influencer (Macchina)
            int currentDay = TotalGameManager.Instance.CurrentDay;
            PlayerPrefs.SetInt($"Day{currentDay}_InfluencerScore", punteggio);
            PlayerPrefs.Save();
            Debug.Log($"[NPC_Influencer] Salvato Day {currentDay} Score = {punteggio}");
        }
        else if (npcName == "Gentlemen")
        {
            // Salva il punteggio del dialogo con NPC Gentlemen (Ponte)
            int currentDay = TotalGameManager.Instance.CurrentDay;
            PlayerPrefs.SetInt($"Day{currentDay}_GentlemenScore", punteggio);
            PlayerPrefs.Save();
            Debug.Log($"[NPC_Gentlemen] Salvato Day {currentDay} Score = {punteggio}");
        }
        else if (npcName == "Friend")
        {
            // Salva il punteggio del dialogo con NPC Friend (Disco)
            int currentDay = TotalGameManager.Instance.CurrentDay;
            PlayerPrefs.SetInt($"Day{currentDay}_FriendScore", punteggio);
            PlayerPrefs.Save();
            Debug.Log($"[NPC_Friend] Salvato Day {currentDay} Score = {punteggio}");
        }
        Debug.Log("Hai scelto NPC: " + npcName);
        Debug.Log("Punteggio: " + punteggio);

        // Chiusura pannello scelte
        ChoicePanel.SetActive(false);
        
    }
}
