using TMPro;
using UnityEngine;

public class PassiveUpgradeSlot : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    private PassiveUpgrades currentUpgrade;

    public void SetUpgrade(PassiveUpgrades upgrade)
    {
        currentUpgrade = upgrade;

        nameText.text = upgrade.itemName;
        descriptionText.text = upgrade.description;
    }

    public PassiveUpgrades GetUpgrade()
    {
        return currentUpgrade;
    }
}