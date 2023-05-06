using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFPHandAnimatorDependencies
{
    Transform HandTransform { get; }
    Transform HandContainer { get; }
}

public class FPHandAnimator : EffectAnimator
{
    private IFPHandAnimatorDependencies _dependencies;
    private Vector3 _handOrigin;

    private void Awake()
    {
        _dependencies = GetComponent<IFPHandAnimatorDependencies>();
        _handOrigin = _dependencies.HandContainer.localPosition;
    }

    public void OnInputStart()
    {
        _dependencies.HandTransform.DOKill();
        _dependencies.HandContainer.DOKill();

        _dependencies.HandTransform.DOShakePosition(1f, 0.01f,
            randomnessMode: ShakeRandomnessMode.Full,
            fadeOut: false,
            vibrato: 20
        ).SetLoops(-1);

        _dependencies.HandContainer.DOLocalMoveZ(_handOrigin.z - 0.1f, 0.45f);
    }

    public void OnInputRelease()
    {
        _dependencies.HandTransform.DOKill();
        _dependencies.HandContainer.DOKill();

        _dependencies.HandContainer.DOLocalMoveZ(_handOrigin.z, 0.15f);
    }
}
