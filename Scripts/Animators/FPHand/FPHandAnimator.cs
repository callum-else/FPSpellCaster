using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFPHandAnimatorDependencies
{
    Transform HandTransform { get; }
    Transform HandContainer { get; }
    UnityEventAggregator EventAggregator { get; }
}

public class FPHandAnimator : MonoBehaviour
{
    private IFPHandAnimatorDependencies _dependencies;
    private Vector3 _handOrigin;

    private void Awake()
    {
        _dependencies = GetComponent<IFPHandAnimatorDependencies>();
        _handOrigin = _dependencies.HandContainer.localPosition;
    }

    private void Start()
    {
        _dependencies.EventAggregator.GetEvent<HandShakeEvent>().Subscribe(OnHandShakeEvent);
        _dependencies.EventAggregator.GetEvent<HandRecoilEvent>().Subscribe(OnHandRecoilEvent);
    }

    private void OnDestroy()
    {
        _dependencies.EventAggregator.GetEvent<HandShakeEvent>().Unsubscribe(OnHandShakeEvent);
        _dependencies.EventAggregator.GetEvent<HandRecoilEvent>().Unsubscribe(OnHandRecoilEvent);
    }

    public void OnHandShakeEvent(HandShakeEventInfo eventInfo)
    {
        EA_DOTweenEventHelper.OnDOTweenEvent(eventInfo,
            kill: () => _dependencies.HandTransform.DOKill(),
            tween: () =>
            {
                return _dependencies.HandTransform.DOShakePosition(
                    duration: eventInfo.Duration,
                    strength: eventInfo.Strength,
                    randomnessMode: ShakeRandomnessMode.Full,
                    fadeOut: false,
                    vibrato: eventInfo.Vibrato
                );
            },
            reset: () => _dependencies.HandTransform.localPosition = Vector3.zero);
    }

    public void OnHandRecoilEvent(HandRecoilEventInfo eventInfo)
    {
        _dependencies.HandContainer.DOKill();

        if (!eventInfo.Kill)
        {
            Tweener tween = null;
            switch (eventInfo.RecoilType)
            {
                case RecoilType.Move:
                    tween = _dependencies.HandContainer.DOLocalMoveZ(
                        endValue: _handOrigin.z - eventInfo.Strength,
                        duration: eventInfo.Duration);
                    break;

                case RecoilType.Punch:
                    tween = _dependencies.HandContainer.DOPunchPosition(
                        punch: Vector3.back * eventInfo.Strength,
                        duration: eventInfo.Duration,
                        vibrato: 0);
                    break;
            }

            if (eventInfo.ResetOnCompleted)
                tween?.OnComplete(() => ResetHandRecoil(eventInfo.Duration));
        }
        else
            ResetHandRecoil(eventInfo.Duration);
    }

    private void ResetHandRecoil(float duration)
    {
        _dependencies.HandContainer.DOLocalMoveZ(
            endValue: _handOrigin.z, 
            duration: duration
        );
    }
}
