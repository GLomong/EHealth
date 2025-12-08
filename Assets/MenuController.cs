using UnityEngine;

public class MenuController : MonoBehaviour
{
    public SceneFade fade;

    public void StartGame()
    {
        fade.FadeToScene("questionarioIniziale");
    }

}