using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUpgradeSlot : MonoBehaviour
{
    public Image passiveUpgradeImage;
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    private PassiveUpgrades currentUpgrade;

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
}