using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    public PlayerController playerController;

    void Update()
    {
        if (!playerController.canClick)
        return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 clickPos = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(clickPos, Vector2.zero);

            if (hit.collider != null)
            {
                ClickToJump button = hit.collider.GetComponent<ClickToJump>();

                if (button != null)
                {
                    playerController.StartJump(button.jumpValue);
                }
            }
        }
    }

}

