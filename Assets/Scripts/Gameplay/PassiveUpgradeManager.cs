using System.Collections.Generic;
using UnityEngine;

public class PassiveUpgradeManager : MonoBehaviour
{
    [Header("All Possible Upgrades")]
    public List<PassiveUpgrades> allUpgrades = new List<PassiveUpgrades>();

    [Header("Upgrade Slots")]
    public List<PassiveUpgradeSlot> upgradeSlots = new List<PassiveUpgradeSlot>();

    [Header("Reference to player")]
    public Player player;
    public PlayerHealth health;
    public PlayerEXP experience;
    public PlayerCurrency currency;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        health = FindFirstObjectByType<PlayerHealth>();
        experience = FindFirstObjectByType<PlayerEXP>();
        currency = FindFirstObjectByType<PlayerCurrency>();
    }

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

    public void ApplyUpgrade(PassiveUpgrades upgrade)
    {
        switch (upgrade.itemName)
        {
            case "HP Up":
                HealthUp(upgrade.effectAmount);
                break;

            case "DMG Up":
                DamageUp(upgrade.effectAmount);
                break;

            case "Speed Up":
                SpeedUp(upgrade.effectAmount);
                break;

            case "Jump Height Up":
                JumpUp(upgrade.effectAmount);
                break;

            case "Critical Chance Up":
                CritUp(upgrade.effectAmount);
                break;

            case "Swing Speed Up":
                MeleeSpeedUp(upgrade.effectAmount);
                break;

            case "Range Up":
                MeleeRangeUp(upgrade.effectAmount);
                break;

            case "EXP Up":
                EXPUp(upgrade.effectAmount);
                break;

            case "Heal":
                EXPUp(upgrade.effectAmount);
                break;

            default:
                Debug.LogWarning("No upgrade function found for: " + upgrade.itemName);
                break;
        }
    }

    public void HealthUp(float effectAmount)
    {
        health.maxHealth = health.maxHealth + effectAmount;
    }
    public void DamageUp(float effectAmount)
    {

    }
    public void SpeedUp(float effectAmount)
    {
        player.speed = player.speed + effectAmount;
    }
    public void JumpUp(float effectAmount)
    {
        player.jumpMultiplier = player.jumpMultiplier + effectAmount;
    }
    public void CritUp(float effectAmount)
    {
    }
    public void MeleeSpeedUp(float effectAmount)
    {
        player.slashDuration = player.slashDuration - effectAmount;
    }
    public void MeleeRangeUp(float effectAmount)
    {
    }
    public void EXPUp(float effectAmount)
    {
        experience.EXPIncrease = experience.EXPIncrease + effectAmount;
    }

    public void Heal(float effectAmount)
    {
        health.health = health.health + effectAmount;
    }
}