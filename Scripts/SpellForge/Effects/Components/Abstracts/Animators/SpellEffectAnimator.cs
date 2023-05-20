using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class SpellEffectAnimator : EffectAnimator
{
    private IEnumerable<EffectAnimationEvent> _castingAnimation;
    private IEnumerable<EffectAnimationEvent> _finishCastingAnimation;
    private IEnumerable<EffectAnimationEvent> _cancelCastingAnimation;

    private bool _isCastable;
    private UnityEvent _onIsCastable = new UnityEvent();

    public bool IsCastable
    {
        get => _isCastable;
        protected set
        {
            _isCastable = value;
            if (value)
                OnIsCastable.Invoke();
        }
    }

    public UnityEvent OnIsCastable
    {
        get => _onIsCastable;
    }

    protected virtual void Awake()
    {
        _castingAnimation = GenerateCastingAnimation();
        _finishCastingAnimation = GenerateFinishCastingAnimation();
        _cancelCastingAnimation = GenerateCancelCastingAnimation();
    }

    public void PlayCastingAnimation()
    {
        InitialiseCastingAnimationComponents();
        SetAnimatorAndPlay(_castingAnimation);
    }

    public void PlayFinishedCastingAnimation()
    {
        InitialiseFinishCastingAnimationComponents();
        SetAnimatorAndPlay(_finishCastingAnimation);
    }

    public void PlayCancelCastingAnimation()
    {
        InitialiseCancelCastingAnimationComponents();
        SetAnimatorAndPlay(_cancelCastingAnimation);
    }

    protected abstract IEnumerable<EffectAnimationEvent> GenerateCastingAnimation();
    protected abstract void InitialiseCastingAnimationComponents();
    protected abstract IEnumerable<EffectAnimationEvent> GenerateFinishCastingAnimation();
    protected abstract void InitialiseFinishCastingAnimationComponents();
    protected abstract IEnumerable<EffectAnimationEvent> GenerateCancelCastingAnimation();
    protected abstract void InitialiseCancelCastingAnimationComponents();
}
