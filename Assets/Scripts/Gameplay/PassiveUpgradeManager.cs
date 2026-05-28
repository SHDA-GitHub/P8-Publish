using System.Collections.Generic;
using UnityEngine;

public class PassiveUpgradeManager : MonoBehaviour
{
    [Header("All Possible Upgrades")]
    public List<PassiveUpgrades> allUpgrades = new List<PassiveUpgrades>();

    [Header("Upgrade Slots")]
    public List<PassiveUpgradeSlot> upgradeSlots = new List<PassiveUpgradeSlot>();

    public void GenerateRandomUpgrades()
    {
        List<PassiveUpgrades> availableUpgrades = new List<PassiveUpgrades>(allUpgrades);

        for (int i = 0; i < upgradeSlots.Count; i++)
        {
            if (availableUpgrades.Count <= 0)
                return;

            int randomIndex = Random.Range(0, availableUpgrades.Count);

            PassiveUpgrades selectedUpgrade = availableUpgrades[randomIndex];

            upgradeSlots[i].SetUpgrade(selectedUpgrade);

            availableUpgrades.RemoveAt(randomIndex);
        }
    }
}