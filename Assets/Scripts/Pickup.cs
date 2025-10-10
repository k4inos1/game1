using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType { Health, FireRate, Damage }
    public PickupType type;
    public int amount = 1;

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            switch (type)
            {
                case PickupType.Health:
                    // implementar si hay salud
                    break;
                case PickupType.FireRate:
                    player.fireRate = Mathf.Max(0.05f, player.fireRate - 0.1f * amount);
                    break;
                case PickupType.Damage:
                    // si PlayerController tuviera daño
                    break;
            }
            Destroy(gameObject);
        }
    }
}
