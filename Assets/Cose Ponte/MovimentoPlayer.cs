using UnityEngine;

public class MovimentoPlayer : MonoBehaviour
{
    public float speed = 2f;
    public float targetX;             
    public BridgeGame game;           

    private bool reachingBridge = true;

    void Update()
    {
        if (reachingBridge)
        {
            if (transform.position.x < targetX)
            {
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
            else
            {
                reachingBridge = false;
                game.StartGame();
            }
        }
    }
}

