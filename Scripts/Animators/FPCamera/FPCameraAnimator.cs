using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFPCameraAnimatorDependencies
{
    UnityEventAggregator EventAggregator { get; }
    Transform CameraTransform { get; }
    Transform CameraContainer { get; }
}

[RequireComponent(typeof(IFPCameraAnimatorDependencies))]
public class FPCameraAnimator : MonoBehaviour
{
    private IFPCameraAnimatorDependencies _dependencies;
    private Vector3 _containerOrigin;

    private void Awake()
    {
        _dependencies = GetComponent<IFPCameraAnimatorDependencies>();
    }

    private void Start()
    {
        _dependencies.EventAggregator.GetEvent<CameraShakeEvent>().Subscribe(OnCameraShakeEvent);
        _dependencies.EventAggregator.GetEvent<CameraPunchEvent>().Subscribe(OnCameraPunchEvent);
        _containerOrigin = _dependencies.CameraContainer.localPosition;
    }

    private void OnDestroy()
    {
        _dependencies.EventAggregator.GetEvent<CameraShakeEvent>().Unsubscribe(OnCameraShakeEvent);
        _dependencies.EventAggregator.GetEvent<CameraPunchEvent>().Unsubscribe(OnCameraPunchEvent);
    }

    private void OnCameraShakeEvent(CameraShakeEventInfo eventInfo)
    {
        EA_DOTweenEventHelper.OnDOTweenEvent(eventInfo,
            kill: () => _dependencies.CameraTransform.DOKill(),
            tween: () =>
            {
                return _dependencies.CameraTransform.DOShakePosition(
                    duration: eventInfo.Duration,
                    strength: eventInfo.Strength,
                    randomnessMode: ShakeRandomnessMode.Full,
                    fadeOut: !(eventInfo as IDOTweenEventInfo).DoesLoop(),
                    vibrato: eventInfo.Vibrato);
            },
            reset: () => _dependencies.CameraTransform.localPosition = Vector3.zero);
    }

    private void OnCameraPunchEvent(CameraPunchEventInfo eventInfo)
    {
        EA_DOTweenEventHelper.OnDOTweenEvent(eventInfo,
            kill: () => _dependencies.CameraContainer.DOKill(),
            tween: () =>
            {
                return _dependencies.CameraContainer.DOPunchPosition(
                    punch: eventInfo.Punch,
                    duration: eventInfo.Duration,
                    vibrato: eventInfo.Vibrato);
            },
            reset: () => _dependencies.CameraContainer.localPosition = _containerOrigin);
    }
}
