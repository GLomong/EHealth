using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public LaneController laneLeft;
    public LaneController laneDown;
    public LaneController laneUp;
    public LaneController laneRight;

    void Update()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            laneLeft.TryHit();

        if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            laneDown.TryHit();

        if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            laneUp.TryHit();

        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            laneRight.TryHit();
    }
}

