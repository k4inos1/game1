using UnityEngine;

public class ExperiencePickup : MonoBehaviour
{
    public int experienceValue = 10;

    void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerController>() as Component;
        if (player == null) player = other.GetComponent<HumanMale>() as Component ?? other.GetComponentInParent<HumanMale>() as Component;
        if (player != null)
        {
            var upgradeSys = FindObjectOfType<UpgradeSystem>();
            if (upgradeSys != null) upgradeSys.AddExperience(experienceValue);
            Destroy(gameObject);
        }
    }
}