using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class PressurePlate : GridElement, IInteractable, IPostInteractable, ISignalEmitter, ISavable, IEditable
{
    public override GridElementType Type => GridElementType.PressurePlate;
    public override bool IsWalkable(Vector2Int _) => true;
    public bool IsPressed { get; private set; }
    public int SignalID { get; private set; }
    public string EditorTitle => "Pressure Plate";

    public void Interact(IMovable _)
    {
        IsPressed = true;
        EmitSignal(IsPressed);
    }
    public void PostInteract(IMovable _)
    {
        IsPressed = false;
        EmitSignal(IsPressed);
    }
    public void EmitSignal(bool signal)
    {
        SignalSystem.Emit(this, signal);
    }

    public ElementState CaptureState()
    {
        return new PressurePlateState 
        { 
            SignalID = this.SignalID 
        };
    }
    public void RestoreState(ElementState state)
    {
        PressurePlateState plateState = (PressurePlateState)state;
        this.SignalID = plateState.SignalID;
    }

    public List<EditorParameter> GetEditorParameters()
    {
        return new List<EditorParameter>
        {
            new()
            {
                Name = "Signal ID",
                Value = SignalID,
                OnChanged = (int id) => SignalID = id
            }
        };
    }
}
[System.Serializable]
public class PressurePlateState : ElementState
{
    public int SignalID;
}