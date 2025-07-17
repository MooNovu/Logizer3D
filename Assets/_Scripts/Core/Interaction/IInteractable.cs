using DG.Tweening;
using UnityEngine;
// ���� ����� ������� ��� ����������� (�� ��� ��� ����, ������������ �������)
public interface IStatic { }
// ��������������� ��� ����� ��������
public interface IInteractable
{
    public void Interact(IMovable movable);
}
// ��������������� ��� ������ � ������
public interface IPostInteractable
{
    public void PostInteract(IMovable movable);
}
// ��������� �� �������, ������� ������ ������� (�������)
public interface ITransformChanger { }
// ����������� ������� (�����, ������ � ��)
public interface ISignalEmitter 
{
    public int SignalID { get; }
    public void EmitSignal(bool signal);
}
// ��������� �������� (�����)
public interface ISignalReciever
{
    public int TargetSignalID { get; }
    public void OnSignalRecieved(bool signal);
}
// ��������� �������� ��� ������
public interface ISpecialAnimation 
{
    public Sequence GetAnimation(Transform targetTransform, Vector2Int targetPosition);
}
// ��������� �������� ������ (������������� )
public interface IExitSpecialAnimation
{
    public Sequence GetExitAnimation(Transform targetTransform, Vector2Int targetPosition);
}
