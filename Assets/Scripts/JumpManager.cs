using UnityEngine;

public class ClickToJump : MonoBehaviour
{
    public PlayerController playerController;
    public int jumpValue=1;   //1,2,3 a seconda di quale bottone è premuto

    //debugging
    private void OnMouseDown()
{
    Debug.Log("Click ricevuto su " + gameObject.name);
}
}

