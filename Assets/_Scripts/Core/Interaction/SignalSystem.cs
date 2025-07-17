using System.Collections.Generic;
using UnityEngine;

public static class SignalSystem
{
    private static List<ISignalReciever> _recievers = new();

    public static void Subscribe(ISignalReciever reciever) => _recievers.Add(reciever);
    public static void Unsubscribe(ISignalReciever reciever) => _recievers.Remove(reciever);
    public static void UnsubscribeAll() => _recievers.Clear();

    public static void Emit(ISignalEmitter emitter, bool signal)
    {
        foreach (ISignalReciever reciever in _recievers)
        {
            if (reciever.TargetSignalID == emitter.SignalID)
            {
                reciever.OnSignalRecieved(signal);
            }
        }
    }

}
