using UnityEngine;

public class Pickup : MonoBehaviour
{
    public enum PickupType { Health, FireRate, Damage }
    public PickupType type;
    public int amount = 1;

    void OnTriggerEnter(Collider other)
    {
        // try to find known player controller types in parent/root
        var human = other.GetComponent<HumanMale>() ?? other.GetComponentInParent<HumanMale>() ?? other.GetComponentInChildren<HumanMale>();

        if (human != null)
        {
            switch (type)
            {
                case PickupType.Health:
                    // if you implement health on HumanMale or PlayerHealth, add here
                    var ph = other.GetComponent<PlayerHealth>() ?? other.GetComponentInParent<PlayerHealth>();
                    if (ph != null) ph.Heal(amount);
                    break;
                case PickupType.FireRate:
                    // HumanMale uses 'fireRate' field
                    human.fireRate = Mathf.Max(0.05f, human.fireRate - 0.1f * amount);
                    break;
                case PickupType.Damage:
                    // if your weapon system exposes damage, apply upgrades there
                    break;
            }
            Destroy(gameObject);
        }
    }
}
