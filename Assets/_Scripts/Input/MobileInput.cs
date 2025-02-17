using UnityEngine;
using UnityEngine.UI;

public class MobileInput : InputHandler
{
    [SerializeField] private Button _upButton;
    [SerializeField] private Button _downButton;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;

    private void Awake()
    {
        //_upButton.onClick.AddListener...
    }
}
