using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public float FadeInSpeed;
    public float FadeOutSpeed;
    [SerializeField]
    private CanvasGroup _canvasGroup;
    private Coroutine _currentActiveFade = null;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        Portal.OnLoadingScene += StartFadeToBlack;
        Portal.OnSceneLoaded += StartFadeToTransparent;
    }

    private void OnDestroy()
    {
        Portal.OnLoadingScene -= StartFadeToBlack;
        Portal.OnSceneLoaded -= StartFadeToTransparent;
    }

    public Coroutine Fade(float target, float time)
    {
        if (_currentActiveFade != null)
            StopCoroutine(_currentActiveFade);
        _currentActiveFade = StartCoroutine(FadeRoutine(target, time));
        return _currentActiveFade;
    }

    private IEnumerator FadeRoutine(float target, float time)
    {
        while(!Mathf.Approximately(_canvasGroup.alpha, target))
        {
            _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.unscaledDeltaTime / time);
            yield return null;
        }
    }

    private void StartFadeToBlack()
    {
        Fade(1, FadeInSpeed);
    }

    private void StartFadeToTransparent()
    {
        Fade(0, .25f);
    }

    
}
