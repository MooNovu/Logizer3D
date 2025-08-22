using DG.Tweening;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    private readonly float _overviewDuration = 2.25f;
    private readonly float _focusExitDuration = 1f;
    private readonly float _focusPlayerDuration = 1f;

    [Inject] private GridSystem _gridSystem;

    private Camera _camera;
    private Vector3 _spawnPosition;
    private Vector3 _exitPosition;

    private Vector3 minPos;
    private Vector3 maxPos;

    private Tween _anim;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
        GameEvents.OnPlayerMoved += FollowPlayer;
    }
    private void OnDestroy()
    {
        GameEvents.OnPlayerMoved -= FollowPlayer;
    }
    private void FollowPlayer(Vector3 pos)
    {
        Vector3 targetPosition = new(pos.x - 3.4f, 10f, pos.z - 5.6f);
        _anim = _camera.transform.DOMove(targetPosition, _focusPlayerDuration).SetEase(Ease.OutQuad);
    }
    public IEnumerator CameraSequence()
    {
        // Ждем завершения загрузки уровня
        yield return new WaitUntil(() => _gridSystem.Width > 0 && _gridSystem.Height > 0);

        // Получаем позиции
        FindSpawnAndExitPosition();

        // 1. Показываем весь уровень
        _anim.Complete();
        yield return MoveCameraToOverview().WaitForCompletion();

        _anim.Complete();
        yield return FocusOnExit().WaitForCompletion();

        // 2. Перемещаемся к игроку
        _anim.Complete();
        yield return FocusOnPlayer().WaitForCompletion();
    }

    private Sequence MoveCameraToOverview()
    {
        //float size = Mathf.Max(maxPos.x - minPos.x, maxPos.z - minPos.z) / 1.5f;
        float height = 15f;//size / Mathf.Tan(_camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        Vector3 startPos = new(maxPos.x, height, maxPos.z - 6f);
        Vector3 endPos = new(minPos.x, height, minPos.z - 6f);

        _camera.transform.SetPositionAndRotation(startPos, Quaternion.Euler(60f, 0, 0));

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_camera.transform.DOMove(endPos, _overviewDuration).SetEase(Ease.InOutQuad));
        _anim = sequence;
        return sequence;
    }
    private Sequence FocusOnExit()
    {
        Vector3 targetPosition = new(_exitPosition.x - 3.4f, 10f, _exitPosition.z - 5.6f);

        Sequence sequence = DOTween.Sequence();

        sequence.Append(_camera.transform.DOMove(targetPosition, _focusExitDuration).SetEase(Ease.InOutQuad));
        sequence.Join(_camera.transform.DORotateQuaternion(Quaternion.Euler(45f, 30f, 0), _focusExitDuration));
        sequence.AppendInterval(0.5f);
        _anim = sequence;
        return sequence;
    }

    private Sequence FocusOnPlayer()
    {
        Vector3 targetPosition = new(_spawnPosition.x - 3.4f, 10f, _spawnPosition.z - 5.6f);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_camera.transform.DOMove(targetPosition, _focusPlayerDuration).SetEase(Ease.InOutQuad));
        sequence.Join(_camera.transform.DORotateQuaternion(Quaternion.Euler(45f, 30f, 0), _focusPlayerDuration));
        _anim = sequence;
        return sequence;
    }

    private void FindSpawnAndExitPosition()
    {
        _spawnPosition = new Vector3(0, 0, 0);
        _exitPosition = new Vector3(0, 0, 0);
        minPos = new(0, 0, 0);
        bool isMinPosSet = false;
        maxPos = new(0, 0, 0);
        for (int x = 0; x < _gridSystem.Width; x++)
        {
            for (int y = 0; y < _gridSystem.Height; y++)
            {
                var cell = _gridSystem.GetCell(new Vector2Int(x, y));
                if (cell.Elements.Count > 0 && !isMinPosSet)
                {
                    minPos = GridSystem.GetWorldPosition(cell.Position);
                    isMinPosSet = true;
                }

                if (cell.Elements.Count > 0) maxPos = GridSystem.GetWorldPosition(cell.Position);

                if (cell.GetFirstElementOfType(GridElementType.Spawn) != null)
                {
                    _spawnPosition = GridSystem.GetWorldPosition(cell.Position);
                }
                if (cell.GetFirstElementOfType(GridElementType.Exit) != null)
                {
                    _exitPosition = GridSystem.GetWorldPosition(cell.Position);
                }
            }
        }
    }
}
