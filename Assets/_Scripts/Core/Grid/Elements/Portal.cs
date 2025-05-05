using ModestTree;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Portal : GridElement
{
    public override GridElementType Type => GridElementType.Portal;
    public override bool IsWalkable => true;
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

    public override void Interact(IMovable movable)
    {
        if (targetPortal == null) targetPortal = FindTargetPortal();
        if (!CheckMoveInto(movable)) return;

        if (targetPortal != null)
        {
            Vector2Int offset = GetPortalTeleportOffset(movable, targetPortal.Rotation);
            movable.SetPreviousPosition(targetPortal.GridPosistion);
            movable.Teleport(targetPortal.GridPosistion + offset);
        }
    }

    private Vector2Int GetPortalTeleportOffset(IMovable movable, int rotation)
    {
        Vector2Int offset = portalTeleportOffset[rotation];
        Vector2Int lastMove = movable.GetLastMoveVector();

        if (lastMove == new Vector2Int(0, -1) || lastMove == new Vector2Int(-1, 0))
        {
            return -offset;
        }
        return offset;
    }
    private bool CheckMoveInto(IMovable movable)
    {
        Vector2Int lastMove = movable.GetLastMoveVector();
        if (Rotation % 2 == 0)
        {
            if (lastMove == new Vector2Int(1, 0) || lastMove == new Vector2Int(-1, 0))
            {
                movable.CancelLastMove();
                return false;
            }
        }
        else
        {
            if (lastMove == new Vector2Int(0, 1) || lastMove == new Vector2Int(0, -1))
            {
                movable.CancelLastMove();
                return false;
            }
        }
        return true;
    }

    public override ElementState CaptureState()
    {
        return new PortalData
        {
            currentPortalId = this.PortalId,
            TargetId = this.targetPortalId
        };
    }
    public override void RestoreState(ElementState state)
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