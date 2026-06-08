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
                HealthUp(upgrade.effectAmountFloat);
                break;

            case "DMG Up":
                DamageUp(upgrade.effectAmountFloat);
                break;

            case "Speed Up":
                SpeedUp(upgrade.effectAmountFloat);
                break;

            case "Jump Height Up":
                JumpUp(upgrade.effectAmountFloat);
                break;

            case "Critical Chance Up":
                CritUp(upgrade.effectAmountFloat);
                break;

            case "Swing Speed Up":
                MeleeSpeedUp(upgrade.effectAmountFloat);
                break;

            case "Range Up":
                MeleeRangeUp(upgrade.effectAmountFloat);
                break;

            case "EXP Up":
                EXPUp(upgrade.effectAmountFloat);
                break;

            case "Heal":
                Heal(upgrade.effectAmountFloat);
                break;

            case "Bullet Speed Up":
                BulletSpeed(upgrade.effectAmountFloat);
                break;

            case "Bullet Capacity Up":
                BulletCapacity(upgrade.effectAmountInt);
                break;

            case "Regeneration":
                Regeneration(upgrade.effectAmountInt);
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
        player.gunDMG = player.gunDMG + effectAmount;
        player.meleeDMG = player.meleeDMG + effectAmount;
        player.UpdateStats();
    }
    public void SpeedUp(float effectAmount)
    {
        player.speed = player.speed + effectAmount;
        player.originalSpeed = player.originalSpeed + effectAmount;
        player.UpdateStats();
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
        player.slashHitbox = player.slashHitbox + effectAmount;
        player.UpdateStats();
    }

    public void EXPUp(float effectAmount)
    {
        experience.EXPIncrease = experience.EXPIncrease + effectAmount;
    }

    public void Heal(float effectAmount)
    {
        health.health = health.health + effectAmount;
    }

    public void Regeneration(float effectAmount)
    {
        health.regenAmount = health.regenAmount + effectAmount;
    }

    public void BulletSpeed(float effectAmount)
    {
        player.gunSpeed = player.gunSpeed + effectAmount;
    }

    public void BulletCapacity(int effectAmount)
    {
        player.bulletsBeforeJam = player.bulletsBeforeJam + effectAmount;
    }
}