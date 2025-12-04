using UnityEngine;
using TMPro;

public class DisplayVariable : MonoBehaviour
{
    public TextMeshProUGUI uiText;   // ora è TMP
    public PlayerController player;

    void Update()
    {
        if (!player.flag_visualizzazione)
            uiText.text = player.num_mosse.ToString();
        else
        {
            uiText.text = player.ponte_score.ToString();
            player.flag_visualizzazione=false;
        }
    }
}