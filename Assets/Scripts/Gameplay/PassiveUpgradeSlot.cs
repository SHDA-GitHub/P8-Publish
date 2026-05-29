using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUpgradeSlot : MonoBehaviour
{
    public Image passiveUpgradeImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public PassiveUpgrades currentUpgrade;
    [SerializeField] private LevelUpUI levelUpUI;

    public void SetUpgrade(PassiveUpgrades upgrade)
    {
        currentUpgrade = upgrade;

        passiveUpgradeImage.sprite = upgrade.itemImage;
        nameText.text = upgrade.itemName;
        descriptionText.text = upgrade.description;
    }

    public PassiveUpgrades GetUpgrade()
    {
        return currentUpgrade;
    }

    public void SelectUpgrade()
    {
        levelUpUI.ApplyUpgradesToPlayer(currentUpgrade);
    }
}