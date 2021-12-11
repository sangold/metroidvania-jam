using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Event/GameEvent", fileName = "New GameEvent")]
public class GameEvent: ScriptableObject
{
    private HashSet<GameEventListener> _listeners = new HashSet<GameEventListener>();

    public void Raise()
    {
        foreach (GameEventListener listener in _listeners)
            listener.OnEventRaised();
    }

    public void RegisterListener(GameEventListener listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        _listeners.Remove(listener);
    }
}

public class GameEventWithArg<T> : ScriptableObject
{
    protected HashSet<GameEventWithArgListener<T>> _listeners = new HashSet<GameEventWithArgListener<T>>();

    public void Raise(T data)
    {
        Debug.Log(_listeners.Count);
        foreach (GameEventWithArgListener<T> listener in _listeners)
            listener.OnEventRaised(data);
    }

    public void RegisterListener(GameEventWithArgListener<T> listener)
    {
        _listeners.Add(listener);
    }

    public void UnregisterListener(GameEventWithArgListener<T> listener)
    {
        _listeners.Remove(listener);
    }
}