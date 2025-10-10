using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 5f;

    void Start()
    {
        Invoke("ReturnToPool", lifeTime);
    }

    void OnCollisionEnter(Collision other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        ReturnToPool();
    }

    void ReturnToPool()
    {
        if (ProjectilePool.Instance != null)
        {
            ProjectilePool.Instance.Return(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
