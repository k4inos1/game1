using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public float projectileSpeed = 12f;
    public int damage = 1;
    public Transform muzzle;

    public void Fire(Transform owner)
    {
        if (ProjectilePool.Instance != null && ProjectilePool.Instance.projectilePrefab != null)
        {
            var go = ProjectilePool.Instance.Get(muzzle != null ? muzzle.position : owner.position + owner.forward, Quaternion.identity);
            var rb = go.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = owner.forward * projectileSpeed;
            var proj = go.GetComponent<Projectile>();
            if (proj != null) proj.damage = damage;
        }
        else
        {
            // fallback: instantiate
            Debug.LogWarning("ProjectilePool no está configurado correctamente.");
        }
    }
}
