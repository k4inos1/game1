using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public class Upgrade
    {
        public string name;
        public int level;
        public int maxLevel = 5;
    }

    public List<Upgrade> upgrades = new List<Upgrade>();
    public PlayerController player;
    public WeaponSystem weapon;
    public UpgradeUI upgradeUI;
    public int experience = 0;
    public int experienceThreshold = 100;

    void Start()
    {
        upgrades.Add(new Upgrade { name = "FireRate", level = 0 });
        upgrades.Add(new Upgrade { name = "Damage", level = 0 });
        upgrades.Add(new Upgrade { name = "MoveSpeed", level = 0 });
    }

    public void AddExperience(int amount)
    {
        experience += amount;
        if (experience >= experienceThreshold)
        {
            experience -= experienceThreshold;
            experienceThreshold = Mathf.RoundToInt(experienceThreshold * 1.5f);
            if (upgradeUI != null) upgradeUI.ShowUpgrades();
        }
    }

    public void ApplyUpgrade(string name)
    {
        var u = upgrades.Find(x => x.name == name);
        if (u == null) return;
        if (u.level < u.maxLevel) u.level++;
        ApplyToPlayer(u);
    }

    void ApplyToPlayer(Upgrade u)
    {
        switch (u.name)
        {
            case "FireRate":
                if (player != null) player.fireRate = Mathf.Max(0.1f, player.fireRate - 0.1f);
                break;
            case "Damage":
                if (weapon != null) weapon.damage += 1;
                break;
            case "MoveSpeed":
                if (player != null) player.moveSpeed += 1f;
                break;
        }
    }
}
