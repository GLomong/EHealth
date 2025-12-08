using UnityEngine;

public class ClickToJump : MonoBehaviour
{
    public PlayerController playerController;
    public int jumpValue=1;   //1,2,3 a seconda di quale bottone è premuto

    //debugging
    public void DoJumpFromUI()
    {
        if (playerController != null)
            playerController.StartJump(jumpValue);
        Debug.Log("BOTTONE PREMUTO - PC = " + playerController);
        Debug.Log("BOTTONE: jumpValue = " + jumpValue);
        Debug.Log("BOTTONE: canClick = " + playerController.canClick);


    }

}

