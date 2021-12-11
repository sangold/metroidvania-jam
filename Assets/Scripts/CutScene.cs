using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutScene : MonoBehaviour
{
    [SerializeField]
    private PlayableDirector _director;
    [SerializeField]
    private bool _isSkippable;
    [SerializeField]
    private float _targetSkipTime;

    private bool _isSkipped;
    private bool _hasStarted;
    
    public void StartCutscene()
    {
        _director.time = 0f;
        _director.Play();
        _isSkipped = false;
        _hasStarted = true;
    }

    public void SkipCutscene()
    {
        if (!_isSkippable || _isSkipped || !_hasStarted) return;
        _director.time = _targetSkipTime;
        _isSkipped = true;
    }
}
