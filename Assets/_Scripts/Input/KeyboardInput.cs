using UnityEngine;

public class KeyboardInput : InputHandler
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            InvokeMove(Vector2Int.right);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            InvokeMove(Vector2Int.down);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            InvokeMove(Vector2Int.left);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            InvokeMove(Vector2Int.up);
        }
    }
}
