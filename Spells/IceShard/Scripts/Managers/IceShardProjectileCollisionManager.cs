using UnityEngine;

[RequireComponent(typeof(IIceShardProjectileDependencies))]
public class IceShardProjectileCollisionManager : MonoBehaviour
{
    IIceShardProjectileDependencies _dependencies;

    private void OnEnable()
    {
        if (_dependencies == null)
            _dependencies = GetComponent<IIceShardProjectileDependencies>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _dependencies.ProjectileEffectAnimator.PlayCollisionAnimation();
        
        if (other.TryGetComponent(out IDamageable damageable))
        {
            // ...
        }
    }
}
