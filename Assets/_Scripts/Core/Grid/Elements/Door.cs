using System.Collections.Generic;
using UnityEngine;

public class Door : GridElement, ISignalReciever, ISavable, IEditable
{
    public override GridElementType Type => GridElementType.Door;
    public int TargetSignalID { get; private set; }
    public bool IsOpen { get; private set; }

    public string EditorTitle => "Door";

    public override bool IsWalkable(Vector2Int moveDirection)
    {
        return IsOpen;
    }
    private void Start()
    {
        SignalSystem.Subscribe(this);
        IsOpen = false;
    }

    public void OnSignalRecieved(bool signal)
    {
        IsOpen = signal;
    }

    public ElementState CaptureState()
    {
        return new DoorState
        {
            TargetSignalID = this.TargetSignalID
        };
    }
    public void RestoreState(ElementState state)
    {
        DoorState doorState = (DoorState)state;
        this.TargetSignalID = doorState.TargetSignalID;
    }
    public List<EditorParameter> GetEditorParameters()
    {
        return new List<EditorParameter>
        {
            new()
            {
                Name = "Target Signal ID",
                Value = TargetSignalID,
                OnChanged = (int id) => TargetSignalID = id
            }
        };
    }
}
[System.Serializable]
public class DoorState : ElementState
{
    public int TargetSignalID;
}