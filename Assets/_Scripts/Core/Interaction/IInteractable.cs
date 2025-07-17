using DG.Tweening;
using UnityEngine;
// Если можно сделать его статическим (На нем нет кода, декоративный элемент)
public interface IStatic { }
// Интерактивности при входе наклетку
public interface IInteractable
{
    public void Interact(IMovable movable);
}
// Интерактивность при выходе с клетки
public interface IPostInteractable
{
    public void PostInteract(IMovable movable);
}
// Добавлять на обьекты, которые меняют позицию (Порталы)
public interface ITransformChanger { }
// Рассылатель сигнала (Плита, рычаги и тд)
public interface ISignalEmitter 
{
    public int SignalID { get; }
    public void EmitSignal(bool signal);
}
// Приемники сигналов (Дверь)
public interface ISignalReciever
{
    public int TargetSignalID { get; }
    public void OnSignalRecieved(bool signal);
}
// Необычная анимации при заходе
public interface ISpecialAnimation 
{
    public Sequence GetAnimation(Transform targetTransform, Vector2Int targetPosition);
}
// Необычная анимация выхода (Настраивается )
public interface IExitSpecialAnimation
{
    public Sequence GetExitAnimation(Transform targetTransform, Vector2Int targetPosition);
}
