using System.Collections;
using UnityEngine;

[RequireComponent(typeof(IIceShardProjectileDependencies))]
public class IceShardProjectileManager : ProjectileManager
{
    [Header("Physics")]
    [SerializeField, Tooltip("Meters per second.")] private float _targetVelocity;

    [Header("Settings")]
    [SerializeField] private float _lifetimeSeconds;
    
    private IIceShardProjectileDependencies _dependencies;
    private bool _canTimeout;
    private bool _isInitialised;

    private void OnEnable()
    {
        if (!_isInitialised)
        {
            _dependencies = GetComponent<IIceShardProjectileDependencies>();
            _isInitialised = true;
        }
    }

    private void OnDisable()
    {
        _canTimeout = false;
        StopAllCoroutines();
    }

    private IEnumerator TimeoutProjectile()
    {
        yield return new WaitForSeconds(_lifetimeSeconds);
        if (_canTimeout)
            _dependencies.ProjectileEffectAnimator.PlayTimeoutAnimation();
    }

    public override void FireProjectile()
    {
        _dependencies.ProjectileRigidbody.AddForce(_dependencies.ProjectileRigidbody.transform.forward * 
            (_dependencies.ProjectileRigidbody.mass * (_targetVelocity / 0.01f)));

        _canTimeout = true;
        StartCoroutine(TimeoutProjectile());
    }

    public override void DisableProjectile()
    {
        _dependencies.ProjectileEffectAnimator.PlayDisableAnimaion();
    }
}
