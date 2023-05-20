using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class IceShardSegmentEffectAnimator : SpellEffectAnimator
{
    [SerializeField] private ParticleSystem _castingParticles;
    [SerializeField] private ParticleSystemForceField _castingParticlesForceField;
    [SerializeField] private ParticleSystem _spellRingBurstParticles;
    [SerializeField] private Renderer _spellRing1Renderer;
    [SerializeField] private Renderer _spellRing2Renderer;
    [SerializeField] private ObjectPool _iceShardProjectilePool;

    private IIceShardProjectileDependencies _projectileAnimatedComponents;
    private Vector3? _iceShardInitialScale = null;

    private float _spellRing1BurstRadius = 0.225f;
    private float _spellRing2BurstRadius = 0.325f;
    private float _iceShardBurstRadius = 0.1f;

    private Transform _projectileParent;
    private Transform _poolParent;

    private UnityEventAggregator _eventAggregator;

    protected override void Awake()
    {
        _eventAggregator = UnityEventAggregator.GetInstance();

        var ring1Mat = new Material(_spellRing1Renderer.material);
        _spellRing1Renderer.material = ring1Mat;
        SetRendererAlpha(_spellRing1Renderer, 0f);

        var ring2Mat = new Material(_spellRing2Renderer.material);
        _spellRing2Renderer.material = ring2Mat;
        SetRendererAlpha(_spellRing2Renderer, 0f);

        var projectileObject = _iceShardProjectilePool.GetPooledObject();
        _projectileAnimatedComponents = _iceShardProjectilePool.TryExtractComponent<IIceShardProjectileDependencies>(projectileObject);
        _iceShardInitialScale = _projectileAnimatedComponents.IceShardTransform.localScale;
        projectileObject.SetActive(false);

        _projectileParent = new GameObject($"{gameObject.GetInstanceID()}_{gameObject.name}_{_projectileAnimatedComponents.ProjectileManager.name}").transform;
        _poolParent = _iceShardProjectilePool.GetPooledParent();

        base.Awake();
    }

    private void SetSpellRingBurstParticleRadius(float radius)
    {
        _spellRingBurstParticles.Stop();
        var particleShape = _spellRingBurstParticles.shape;
        particleShape.radius = radius;
    }

    private void SetRendererAlpha(Renderer renderer, float alpha)
    {
        var col = renderer.material.color;
        col.a = alpha;
        renderer.material.color = col;
    }

    protected override void InitialiseCastingAnimationComponents()
    {
        var projectileObject = _iceShardProjectilePool.GetPooledObject();
        _projectileAnimatedComponents = _iceShardProjectilePool.TryExtractComponent<IIceShardProjectileDependencies>(projectileObject);

        _projectileAnimatedComponents.IceShardRenderer.enabled = true;
        _projectileAnimatedComponents.ProjectileCollider.enabled = false;
        _projectileAnimatedComponents.ProjectileTrail.Clear();
        _projectileAnimatedComponents.ProjectileTrail.enabled = false;
        _projectileAnimatedComponents.ProjectileRigidbody.isKinematic = true;
        _projectileAnimatedComponents.RootObject.transform.parent = _poolParent;
        _projectileAnimatedComponents.IceShardTransform.localScale = Vector3.zero;
        _projectileAnimatedComponents.RootObject.transform.localPosition = Vector3.zero;
        _projectileAnimatedComponents.RootObject.transform.localEulerAngles = Vector3.zero;

        _castingParticlesForceField.enabled = true;
        SetRendererAlpha(_spellRing1Renderer, 0f);
        SetRendererAlpha(_spellRing2Renderer, 0f);
    }

    protected override IEnumerable<EffectAnimationEvent> GenerateCastingAnimation()
    {
        var castingAnimation = new List<EffectAnimationEvent>();

        var event1 = new EffectAnimationEvent() { DurationSeconds = 0.15f };
        event1.AnimationEvent.AddListener(() => 
        {
            SetSpellRingBurstParticleRadius(_spellRing1BurstRadius);
            _spellRingBurstParticles.Play();
            _spellRing1Renderer.material.DOFade(1f, event1.DurationSeconds);
            _castingParticles.Play();
        });
        castingAnimation.Add(event1);

        var event2 = new EffectAnimationEvent() { DurationSeconds = 0.15f };
        event2.AnimationEvent.AddListener(() => 
        {
            SetSpellRingBurstParticleRadius(_spellRing2BurstRadius);
            _spellRingBurstParticles.Play();
            _spellRing2Renderer.material.DOFade(1f, event2.DurationSeconds);
        });
        castingAnimation.Add(event2);

        var event3 = new EffectAnimationEvent() { DurationSeconds = 0.15f };
        event3.AnimationEvent.AddListener(() => 
        {
            _projectileAnimatedComponents.SmokeParticles.Play();
            _projectileAnimatedComponents.IceShardTransform.DOScale(_iceShardInitialScale.Value, event3.DurationSeconds);
            _projectileAnimatedComponents.IceShardTransform.DOShakePosition(event3.DurationSeconds, 0.015f, 
                randomnessMode: ShakeRandomnessMode.Harmonic, fadeOut: false, vibrato: 20).SetLoops(-1);
        });
        castingAnimation.Add(event3);

        var event4 = new EffectAnimationEvent();
        event4.AnimationEvent.AddListener(() => 
        {
            SetSpellRingBurstParticleRadius(_iceShardBurstRadius);
            _spellRingBurstParticles.Play();
            IsCastable = true;
        });
        castingAnimation.Add(event4);

        return castingAnimation;
    }
        
    private EffectAnimationEvent GenerateDispurseAnimationEvent(float duration)
    {
        var event1 = new EffectAnimationEvent();
        event1.AnimationEvent.AddListener(() => 
        {
            IsCastable = false;

            _castingParticlesForceField.enabled = false;
            _spellRing1Renderer.material.DOFade(0f, duration);
            _spellRing2Renderer.material.DOFade(0f, duration);

            _castingParticles.Stop();
        });
        return event1;
    }

    private void InitialiseDispurseAnimation()
    {
        _spellRing1Renderer.material.DOKill();
        _spellRing2Renderer.material.DOKill();
    }

    protected override IEnumerable<EffectAnimationEvent> GenerateFinishCastingAnimation()
    {
        var dispurseEvent = GenerateDispurseAnimationEvent(0.25f);
        dispurseEvent.AnimationEvent.AddListener(() => 
        {
            _projectileAnimatedComponents.RootObject.transform.parent = _projectileParent;
            _projectileAnimatedComponents.ProjectileTrail.enabled = true;
            _projectileAnimatedComponents.ProjectileCollider.enabled = true;
            _projectileAnimatedComponents.ProjectileRigidbody.isKinematic = false;
            _projectileAnimatedComponents.ProjectileManager.FireProjectile();
        });
        return new List<EffectAnimationEvent>() { dispurseEvent };
    }

    protected override void InitialiseFinishCastingAnimationComponents() 
        => InitialiseDispurseAnimation();

    protected override IEnumerable<EffectAnimationEvent> GenerateCancelCastingAnimation()
    {
        var cancelEvent = GenerateDispurseAnimationEvent(0.25f);
        cancelEvent.AnimationEvent.AddListener(() => 
        {
            _projectileAnimatedComponents.ProjectileManager.DisableProjectile();
        });
        return new List<EffectAnimationEvent>() { cancelEvent };
    }

    protected override void InitialiseCancelCastingAnimationComponents() 
        => InitialiseDispurseAnimation();
}