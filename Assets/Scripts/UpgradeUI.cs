using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public GameObject upgradePanel;
    public Button[] upgradeButtons;
    public Text[] upgradeTexts;
    public UpgradeSystem upgradeSystem;

    private string[] upgradeNames = { "FireRate", "Damage", "MoveSpeed" };

    void Start()
    {
        upgradePanel.SetActive(false);
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int index = i;
            upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(index));
        }
    }

    public void ShowUpgrades()
    {
        upgradePanel.SetActive(true);
        Time.timeScale = 0; // Pausar juego
        for (int i = 0; i < upgradeTexts.Length; i++)
        {
            var u = upgradeSystem.upgrades.Find(x => x.name == upgradeNames[i]);
            upgradeTexts[i].text = upgradeNames[i] + " (Lv " + u.level + ")";
        }
    }

    void SelectUpgrade(int index)
    {
        upgradeSystem.ApplyUpgrade(upgradeNames[index]);
        upgradePanel.SetActive(false);
        Time.timeScale = 1; // Reanudar
    }
}