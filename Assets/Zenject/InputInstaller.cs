using Zenject;
using UnityEngine;

public class InputInstaller : MonoInstaller
{
    [SerializeField] private MobileInput _mobileInputPrefab;
    [SerializeField] private KeyboardInput _keyboardInputPrefab;

    public override void InstallBindings()
    {
        Container.Bind<InputHandler>().FromMethod(GetInputHandler).AsSingle();
    }

    private InputHandler GetInputHandler()
    {
        return Application.isMobilePlatform ? Instantiate(_mobileInputPrefab) : Instantiate(_keyboardInputPrefab);
    }
}
