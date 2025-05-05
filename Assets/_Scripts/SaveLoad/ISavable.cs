using UnityEngine;

public interface ISavable
{
    public ElementState CaptureState();
    public void RestoreState(ElementState state);
}
