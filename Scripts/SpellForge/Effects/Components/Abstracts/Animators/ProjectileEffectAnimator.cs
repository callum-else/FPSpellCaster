using System;
using System.Collections.Generic;

public abstract class ProjectileEffectAnimator : EffectAnimator
{
    private IEnumerable<EffectAnimationEvent> _collisionAnimation;
    private IEnumerable<EffectAnimationEvent> _timeoutAnimation;
    private IEnumerable<EffectAnimationEvent> _disableAnimation;

    protected virtual void Awake()
    {
        _collisionAnimation = GenerateCollisionAnimation();
        _timeoutAnimation = GenerateTimeoutAnimation();
        _disableAnimation = GenerateDisableAnimation();
    }

    public void PlayCollisionAnimation()
    {
        SetAnimatorAndPlay(_collisionAnimation);
    }

    public void PlayTimeoutAnimation()
    {
        SetAnimatorAndPlay(_timeoutAnimation);
    }

    public void PlayDisableAnimaion()
    {
        SetAnimatorAndPlay(_disableAnimation);
    }

    protected abstract IEnumerable<EffectAnimationEvent> GenerateCollisionAnimation();
    protected abstract IEnumerable<EffectAnimationEvent> GenerateTimeoutAnimation();
    protected abstract IEnumerable<EffectAnimationEvent> GenerateDisableAnimation();
}
