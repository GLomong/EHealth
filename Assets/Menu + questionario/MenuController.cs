using UnityEngine;

public class MenuController : MonoBehaviour
{
    public SceneFade fade;

    //Quando viene premuto il bottone 'StartGame' inizia la scena successiva del questionario.
    public void StartGame()
    {
        fade.FadeToScene("Questionario");
    }

}