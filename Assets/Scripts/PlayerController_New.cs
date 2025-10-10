using UnityEngine;

public class PlayerController_New : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float fireRate = 0.5f;
    private float fireTimer = 0f;

    public WeaponSystem weaponSystem;
    public PlayerHealth health;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);

        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            if (weaponSystem != null) weaponSystem.Fire(transform);
        }
    }
}
