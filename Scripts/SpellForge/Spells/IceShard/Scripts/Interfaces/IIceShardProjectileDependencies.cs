using UnityEngine;

public interface IIceShardProjectileDependencies
{
    public GameObject RootObject { get; }
    public Rigidbody ProjectileRigidbody { get; }
    public ProjectileManager ProjectileManager { get; }
    public ProjectileEffectAnimator ProjectileEffectAnimator { get; }
    public ParticleSystem SmokeParticles { get; }
    public Transform IceShardTransform { get; }
    public TrailRenderer ProjectileTrail { get; }
    public Renderer IceShardRenderer { get; }
    public Collider ProjectileCollider { get; }
    public ParticleSystem SmokeBurstParticles { get; }
    public ParticleSystem ShardBurstParticles { get; }
}