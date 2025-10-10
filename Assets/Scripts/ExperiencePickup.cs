using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public int experienceValue = 10;

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>();
        if (player != null)
        {
            var upgradeSys = FindObjectOfType<UpgradeSystem>();
            if (upgradeSys != null) upgradeSys.AddExperience(experienceValue);
            Destroy(gameObject);
        }
    }
}