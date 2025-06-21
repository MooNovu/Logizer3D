using ModestTree;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class Portal : GridElement, ISavable, IInteractable
{
    [Inject] private GridSystem _gridSystem;
    public override GridElementType Type => GridElementType.Portal;
    [SerializeField] private int PortalId;
    [SerializeField] private int targetPortalId;

    private readonly Dictionary<int, Vector2Int> portalTeleportOffset = new()
    {
        {0, new Vector2Int(0, -1) },
        {1, new Vector2Int(-1, 0) },
        {2, new Vector2Int(0, 1) },
        {3, new Vector2Int(1, 0) }
    };

    private Portal targetPortal;
    public override bool IsWalkable(Vector2Int moveDirection)
    {
        bool isCorrectEntryDirection;
        if (Rotation % 2 == 0) isCorrectEntryDirection = moveDirection.y != 0;
        else isCorrectEntryDirection = moveDirection.x != 0;
        if (!isCorrectEntryDirection) return false;

        if (targetPortal == null) targetPortal = FindTargetPortal();
        if (targetPortal == null) return false;

        Vector2Int exitOffset = GetPortalTeleportOffset(moveDirection, targetPortal.Rotation);
        Vector2Int exitPosition = targetPortal.GridPosistion + exitOffset;
        return _gridSystem.IsCellWalkable(exitPosition, exitOffset);
    }
    public void Interact(IMovable movable)
    {
        if (targetPortal == null) targetPortal = FindTargetPortal();
        if (targetPortal == null) return;

        Vector2Int offset = GetPortalTeleportOffset(movable.LastMoveVector, targetPortal.Rotation);
        movable.PreviousPosition = targetPortal.GridPosistion;
        movable.TryTeleport(targetPortal.GridPosistion + offset);
    }

    private Vector2Int GetPortalTeleportOffset(Vector2Int lastMove, int rotation)
    {
        Vector2Int offset = portalTeleportOffset[rotation];

        if (lastMove == new Vector2Int(0, -1) || lastMove == new Vector2Int(-1, 0))
        {
            return -offset;
        }
        return offset;
    }
    public ElementState CaptureState()
    {
        return new PortalData
        {
            currentPortalId = this.PortalId,
            TargetId = this.targetPortalId
        };
    }
    public void RestoreState(ElementState state)
    {
        PortalData data = (PortalData)state;
        PortalId = data.currentPortalId;
        targetPortalId = data.TargetId;
    }
    private Portal FindTargetPortal()
    {
        Portal[] portals = FindObjectsByType<Portal>(FindObjectsSortMode.None);
        foreach (var portal in portals)
        {
            if (portal.PortalId == targetPortalId) return portal;
        }
        return null;
    }
}

[System.Serializable]
public class PortalData : ElementState
{
    public int currentPortalId;
    public int TargetId;
}