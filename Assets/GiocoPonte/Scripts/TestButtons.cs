using UnityEngine;

//Script per vedere se funzionano i bottoni quando clicco:
public class TestButtons : MonoBehaviour
{
    public MovimentoPonte movimento;   

    public void ClickX1()
    {
        Debug.Log("Hai cliccato X1!");
        movimento.Avanza(1);
    }

    public void ClickX2()
    {
        Debug.Log("Hai cliccato X2!");
        movimento.Avanza(2);
    }

    public void ClickX3()
    {
        Debug.Log("Hai cliccato X3!");
        movimento.Avanza(3);
    }
}

