using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField]
    private GameEvent _gameEvent;
    [SerializeField]
    private UnityEvent _responseEvent;

    private void OnEnable()
    {
        _gameEvent.RegisterListener(this);
    }

    private void OnDisable()
    {
        _gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised()
    {
        _responseEvent?.Invoke();
    }
}

public class GameEventWithArgListener<T> : MonoBehaviour
{
    [SerializeField]
    private GameEventWithArg<T> _gameEvent;
    [SerializeField]
    private UnityEvent<T> _responseEvent;

    protected void Awake()
    {
        _gameEvent.RegisterListener(this);
    }

    protected void OnDestroy()
    {
        Debug.Log(this);
        _gameEvent.UnregisterListener(this);
    }

    public void OnEventRaised(T data)
    {
        Debug.Log(data);
        _responseEvent?.Invoke(data);
    }
}
