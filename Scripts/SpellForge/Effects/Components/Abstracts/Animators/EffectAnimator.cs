using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EffectAnimator : MonoBehaviour
{
    private bool _isPlaying = false;
    private float _nextAnimationTime;
    private float _previousAnimationDuration;
    private IEnumerator<EffectAnimationEvent> _eventEnumerator;

    public bool IsPlaying
    {
        get => _isPlaying;
    }

    protected void Play()
    {
        if (_eventEnumerator != null && (_eventEnumerator.Current != null || _eventEnumerator.MoveNext()))
        {
            _nextAnimationTime = Time.time + _eventEnumerator.Current.TriggerAfterSeconds;
            _isPlaying = true;
        }
        else
            print("Could not play effect animator.");
    }

    protected void Stop()
    {
        _isPlaying = false;
    }

    protected void SetAnimator(IEnumerable<EffectAnimationEvent> animationEvents)
    {
        Stop();
        _nextAnimationTime = 0;
        _previousAnimationDuration = 0;
        _eventEnumerator = animationEvents.GetEnumerator();
    }

    protected void SetAnimatorAndPlay(IEnumerable<EffectAnimationEvent> animationEvents)
    {
        SetAnimator(animationEvents);
        Play();
    }

    private void FixedUpdate()
    {
        if (_isPlaying && Time.time >= _nextAnimationTime)
        {
            _eventEnumerator.Current.AnimationEvent.Invoke();
            _previousAnimationDuration = _eventEnumerator.Current.DurationSeconds;

            if (!_eventEnumerator.MoveNext())
            {
                _isPlaying = false;
                return;
            }

            _nextAnimationTime += _previousAnimationDuration + _eventEnumerator.Current.TriggerAfterSeconds;
        }
    }
}

[Serializable]
public class EffectAnimationEvent
{
    public float TriggerAfterSeconds = 0f;
    public float DurationSeconds = 0f;
    public UnityEvent AnimationEvent = new UnityEvent();
}
