using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class DialogBoxTimeline : MonoBehaviour, ITimeControl
{
    [SerializeField]
    private TextMeshProUGUI _textMeshActor;
    [SerializeField]
    private TextMeshProUGUI _textMeshContent;
    [SerializeField]
    private string _actorName;
    [SerializeField]
    private string _text;

    private float _startTime = 0;

    public void SetTime(double time)
    {
        if(_startTime == 0)
            _startTime = (float)time;
        _textMeshContent.maxVisibleCharacters = Mathf.FloorToInt(((float)time - _startTime) / .05f);
    }

    public void OnControlTimeStart()
    {
        _startTime = 0;
        _textMeshActor.text = _actorName;
        _textMeshContent.text = _text;
        _textMeshContent.maxVisibleCharacters = 0;
    }

    public void OnControlTimeStop()
    {
        
    }
}
