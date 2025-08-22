using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Candy : GridElement, IInteractable
{
    public override GridElementType Type => GridElementType.Candy;
    public override bool IsWalkable(Vector2Int _) => true;

    private bool IsCollected = false;

    private float rotationDuration = 2f;
    private float moveDuration = 1f;
    private float moveHeight = 0.2f;

    private Vector3 _originalPosition;

    private Sequence _moveAnim;

    private void Start()
    {
        GameEvents.OnLevelFullLoad += StartAnimSequence;
    }
    private void OnDestroy()
    {
        StopAnimations();
        GameEvents.OnLevelFullLoad -= StartAnimSequence;
    }
    public void Interact(IMovable movable)
    {
        if (IsCollected == true) return;


        if (movable is PlayerMover)
        {
            GameEvents.PlayerPickedCandy();
            IsCollected = true;
            StopAnimations();
        }
    }

    private void StartAnimSequence()
    {
        StartCoroutine(StartAnim());
    }
    private IEnumerator StartAnim()
    {
        _originalPosition = transform.position;
        yield return UpObject().WaitForCompletion();
        _originalPosition = transform.position;
        RotateObject();
        MoveObject();
    }
    private Sequence UpObject()
    {
        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOMoveY(_originalPosition.y + 0.22f, moveDuration).SetEase(Ease.InOutSine));
        moveSequence.Append(transform.GetChild(0).transform.DORotate(new Vector3(-70f, 0, 0), moveDuration));
        _moveAnim = moveSequence;
        return moveSequence;
    }
    private void RotateObject()
    {
        transform.DORotate(new Vector3(0, 360, 0), rotationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }
    private void MoveObject()
    {
        Sequence moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOMoveY(_originalPosition.y + moveHeight, moveDuration).SetEase(Ease.InOutSine));
        moveSequence.Append(transform.DOMoveY(_originalPosition.y, moveDuration).SetEase(Ease.InOutSine));
        moveSequence.SetLoops(-1);
        _moveAnim = moveSequence;
    }


    private void StopAnimations()
    {
        _moveAnim.Kill();
        DOTween.Kill(transform);
    }
}
