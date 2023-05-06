using UnityEngine;

public class IceShardProjectileDependencyProvider : MonoBehaviour, IIceShardProjectileDependencies
{
    [SerializeField] private ProjectileManager _projectileManager;
    [SerializeField] private ProjectileEffectAnimator _projectileEffectAnimator;
    [SerializeField] private Rigidbody _projectileRigidbody;
    [SerializeField] private ParticleSystem _smokeParticles;
    [SerializeField] private Transform _iceShardTransform;
    [SerializeField] private TrailRenderer _projectileTrail;
    [SerializeField] private Renderer _iceShardRenderer;
    [SerializeField] private Collider _projectileCollider;
    [SerializeField] private ParticleSystem _smokeBurstParticles;
    [SerializeField] private ParticleSystem _shardBurstParticles;

    public GameObject RootObject => gameObject;
    public ProjectileManager ProjectileManager => _projectileManager;
    public ProjectileEffectAnimator ProjectileEffectAnimator => _projectileEffectAnimator;
    public Rigidbody ProjectileRigidbody => _projectileRigidbody;
    public ParticleSystem SmokeParticles => _smokeParticles;
    public Transform IceShardTransform => _iceShardTransform;
    public TrailRenderer ProjectileTrail => _projectileTrail;
    public Renderer IceShardRenderer => _iceShardRenderer;
    public Collider ProjectileCollider => _projectileCollider;
    public ParticleSystem SmokeBurstParticles => _smokeBurstParticles;
    public ParticleSystem ShardBurstParticles => _shardBurstParticles;
}
