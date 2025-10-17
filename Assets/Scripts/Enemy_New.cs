using UnityEngine;
using UnityEngine.AI;

public class Enemy_New : MonoBehaviour
{
    public float speed = 2f; // used if no NavMeshAgent
    public int health = 1;
    public int contactDamage = 1;
    public GameObject deathVFX;
    public AudioClip deathSound;
    public GameObject experiencePickupPrefab;
    private AudioSource audioSource;

    Transform target;
    NavMeshAgent agent;

    void Start()
    {
        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null) target = playerObj.transform;
        audioSource = GetComponent<AudioSource>();

        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            // configure agent with fallback values
            agent.speed = Mathf.Max(0.1f, speed);
            agent.updateRotation = true;
            agent.updateUpAxis = true;
        }
    }

    void Update()
    {
        if (target == null) return;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            // fallback movement when no NavMesh is available
            Vector3 dir = (target.position - transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime, Space.World);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        var player = other.gameObject.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(contactDamage);
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) Die();
    }

    void Die()
    {
        if (deathVFX != null) Instantiate(deathVFX, transform.position, Quaternion.identity);
        if (audioSource != null && deathSound != null) audioSource.PlayOneShot(deathSound);
        if (experiencePickupPrefab != null) Instantiate(experiencePickupPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
