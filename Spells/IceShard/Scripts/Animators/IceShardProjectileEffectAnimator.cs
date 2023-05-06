using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(IIceShardProjectileDependencies))]
public class IceShardProjectileEffectAnimator : ProjectileEffectAnimator
{
    IIceShardProjectileDependencies _dependencies;

    protected override void Awake() 
    { 
        _dependencies = GetComponent<IIceShardProjectileDependencies>();
        base.Awake();
    }

    private List<EffectAnimationEvent> GetProjectileDisabledBaseAnimation()
    {
        var disableAnimation = new List<EffectAnimationEvent>();
        var event1 = new EffectAnimationEvent() { DurationSeconds = _dependencies.ProjectileTrail.time };
        event1.AnimationEvent.AddListener(() =>
        {
            _dependencies.SmokeParticles.Stop();
            _dependencies.ProjectileRigidbody.isKinematic = true;
        });
        disableAnimation.Add(event1);

        var event2 = new EffectAnimationEvent();
        event2.DurationSeconds = Mathf.Abs(_dependencies.SmokeParticles.main.startLifetime.constant - _dependencies.ProjectileTrail.time);
        event2.AnimationEvent.AddListener(() =>
        {
            _dependencies.ProjectileTrail.Clear();
            _dependencies.ProjectileTrail.enabled = false;
        });
        disableAnimation.Add(event2);

        var event3 = new EffectAnimationEvent();
        event3.AnimationEvent.AddListener(() =>
        {
            _dependencies.RootObject.gameObject.SetActive(false);
        });
        disableAnimation.Add(event3);

        return disableAnimation;
    }

    protected override IEnumerable<EffectAnimationEvent> GenerateCollisionAnimation()
    {
        var collisionAnimation = GetProjectileDisabledBaseAnimation();
        collisionAnimation.First().AnimationEvent.AddListener(() => 
        {
            _dependencies.IceShardRenderer.enabled = false;
            _dependencies.SmokeBurstParticles.Play();
            _dependencies.ShardBurstParticles.Play();
        });
        return collisionAnimation;
    }

    protected override IEnumerable<EffectAnimationEvent> GenerateTimeoutAnimation()
    {
        var timeOutAnimation = GetProjectileDisabledBaseAnimation();
        timeOutAnimation.First().AnimationEvent.AddListener(() => 
        {
            _dependencies.IceShardRenderer.enabled = false;
            _dependencies.ShardBurstParticles.Play(); 
        });
        return timeOutAnimation;
    }

    protected override IEnumerable<EffectAnimationEvent> GenerateDisableAnimation()
    {
        var disableAnimation = new List<EffectAnimationEvent>();
        
        var event1 = new EffectAnimationEvent() { DurationSeconds = 0.5f };
        event1.AnimationEvent.AddListener(() => 
        {
            _dependencies.IceShardTransform.DOKill();
            _dependencies.IceShardTransform.DOScale(0f, event1.DurationSeconds);
        });

        disableAnimation.Add(event1);
        disableAnimation.AddRange(GetProjectileDisabledBaseAnimation());

        return disableAnimation;
    }
}