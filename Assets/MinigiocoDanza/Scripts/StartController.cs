using UnityEngine;

public class StartController : MonoBehaviour
{
    public GameObject startPanel;
    public Behaviour[] scriptsToDisable;

    void Start()
    {
        startPanel.SetActive(true);

        foreach (var s in scriptsToDisable)
            s.enabled = false;   // disattiva gli script del gameplay
    }

    public void OnStartPressed()
    {
        foreach (var s in scriptsToDisable)
            s.enabled = true;    // riattiva gli script

        startPanel.SetActive(false);
    }
}