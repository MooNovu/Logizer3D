using UnityEngine;
using Zenject;

public class PlayerMover : Mover
{
    [Inject] private InputHandler _inputHandler;

    private void OnEnable()
    {
        _inputHandler.OnMovuInput += Move;
    }
    private void OnDisable()
    {
        _inputHandler.OnMovuInput -= Move;
    }
    public void InitializePlayerPosition()
    {
        //
    }
}
