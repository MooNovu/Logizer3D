using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMover : Mover
{
    private IInputProvider _input;

    private void Awake()
    {
        _input = GetComponent<IInputProvider>();
    }

    private void OnEnable()
    {
        _input.OnMove += Move;
    }
    private void OnDisable()
    {
        _input.OnMove -= Move;
    }
    public void InitializePlayerPosition()
    {
        //
    }
}
