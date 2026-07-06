using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveUpgradeManager : MonoBehaviour
{
    [Header("All Possible Upgrades")]
    public List<PassiveUpgrades> allUpgrades = new List<PassiveUpgrades>();

    [Header("Upgrade Slots")]
    public List<PassiveUpgradeSlot> upgradeSlots = new List<PassiveUpgradeSlot>();

    [Header("Chosen Upgrades")]
    public List<PassiveUpgrades> MaxCappedSlots = new List<PassiveUpgrades>();
    private const int maxCappedUpgrades = 4;

    [Header("Temporary Vending Upgrades")]
    public List<PassiveUpgrades> temporaryVendingUpgrades = new();

    [Header("Upgrade Images")]
    public List<GameObject> passiveUpgrades;
    public List<GameObject> tempUpgrades;

    [Header("Upgrade Updating UI")]
    public TextMeshProUGUI upgradeText;
    [SerializeField] private float visibleDuration = 1f;
    [SerializeField] private float fadeDuration = 2f;

    private Coroutine fadeCoroutine;

    private float lastWaveChecked;

    [Header("Reference to player")]
    public Player player;
    public PlayerHealth health;
    public PlayerEXP experience;
    public PlayerCurrency currency;
    public WaveManager waveManager;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        health = FindFirstObjectByType<PlayerHealth>();
        experience = FindFirstObjectByType<PlayerEXP>();
        currency = FindFirstObjectByType<PlayerCurrency>();
        waveManager = FindFirstObjectByType<WaveManager>();

        lastWaveChecked = waveManager.CurrentWave;
        upgradeText.text = "";
        upgradeText.alpha = 0f;
    }

    private void Update()
    {
        if (waveManager.CurrentWave > lastWaveChecked)
        {
            RemoveExpiredVendingUpgrades();

            lastWaveChecked = waveManager.CurrentWave;
        }
    }

    public void RefreshPassiveUpgradeImages()
    {
        for (int i = 0; i < passiveUpgrades.Count; i++)
        {
            Image img = passiveUpgrades[i].GetComponent<Image>();

            if (i < MaxCappedSlots.Count)
            {
                img.sprite = MaxCappedSlots[i].itemImage;

                Color c = img.color;
                c.a = 1f;
                img.color = c;
            }
            else
            {
                img.sprite = null;

                Color c = img.color;
                c.a = 0.2f;
                img.color = c;
            }
        }
    }

    public void RefreshTempUpgradeImages()
    {
        for (int i = 0; i < tempUpgrades.Count; i++)
        {
            Image img = tempUpgrades[i].GetComponent<Image>();

            if (i < temporaryVendingUpgrades.Count)
            {
                img.sprite = temporaryVendingUpgrades[i].itemImage;

                Color c = img.color;
                c.a = 1f;
                img.color = c;
            }
            else
            {
                img.sprite = null;

                Color c = img.color;
                c.a = 0.2f;
                img.color = c;
            }
        }
    }

    public void GenerateRandomUpgrades()
    {
        List<PassiveUpgrades> availableUpgrades = new List<PassiveUpgrades>();

        foreach (PassiveUpgrades upgrade in allUpgrades)
        {
            if (!upgrade.hasMaxCap)
            {
                availableUpgrades.Add(upgrade);
            }
            else
            {
                if (MaxCappedSlots.Count < maxCappedUpgrades)
                {
                    availableUpgrades.Add(upgrade);
                }
                else if (MaxCappedSlots.Contains(upgrade))
                {
                    availableUpgrades.Add(upgrade);
                }
            }
        }

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
        if (upgrade.hasMaxCap &&
            !MaxCappedSlots.Contains(upgrade) &&
            MaxCappedSlots.Count < maxCappedUpgrades)
        {
            MaxCappedSlots.Add(upgrade);
            RefreshPassiveUpgradeImages();
        }

        DataCarrying data = FindFirstObjectByType<DataCarrying>();

        if (upgrade.hasMaxCap && data != null)
        {
            data.RecordCappedUpgrade(upgrade.itemName);
        }

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
                CritChanceUp(upgrade.effectAmountFloat);
                break;

            case "Critical Effect Up":
                CritEffectUp(upgrade.effectAmountFloat);
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
                Regeneration(upgrade.effectAmountFloat);
                break;

            case "Currency Increase":
                CurrencyIncrease(upgrade.effectAmountInt);
                break;

            case "Free Money":
                FreeMoney(upgrade.effectAmountInt);
                break;

            case "Knockback Up":
                KnockbackUp(upgrade.effectAmountFloat);
                break;

            case "Enemy Strength Up":
                EnemyStrengthUp(upgrade.effectAmountInt, upgrade.effectAmountFloat);
                break;

            default:
                Debug.LogWarning("No upgrade function found for: " + upgrade.itemName);
                break;
        }
    }

    public void RemoveUpgrade(PassiveUpgrades upgrade)
    {
        switch (upgrade.itemName)
        {
            case "DMG Up":
                player.gunDMG -= upgrade.effectAmountFloat;
                player.meleeDMG -= upgrade.effectAmountFloat;
                break;

            case "Range Up":
                player.slashHitbox -= upgrade.effectAmountFloat;
                break;

            case "Speed Up":
                player.speed -= upgrade.effectAmountFloat;
                player.originalSpeed -= upgrade.effectAmountFloat;
                break;

            case "Currency Increase":
                currency.currencyIncrease -= upgrade.effectAmountInt;
                break;

            case "Knockback Up":
                player.knockbackStrength -= upgrade.effectAmountFloat;
                break;

            case "Bullet Capacity Up":
                player.bulletsBeforeJam -= upgrade.effectAmountInt;
                break;

            case "Bullet Speed Up":
                player.gunSpeed -= upgrade.effectAmountFloat;
                break;

            case "Critical Chance Up":
                player.critChance -= upgrade.effectAmountFloat;
                break;

            case "Critical Effect Up":
                player.critEffect -= upgrade.effectAmountFloat;
                break;

            case "EXP Up":
                experience.EXPIncrease -= upgrade.effectAmountFloat;
                break;

            case "Regeneration":
                health.regenAmount -= upgrade.effectAmountFloat;
                break;
        }

        player.UpdateStats();
    }

    public void ApplyTemporaryUpgrade(PassiveUpgrades upgrade)
    {
        ApplyUpgrade(upgrade);
        ShowUpgradeText($"+ {upgrade.itemName}");
        if (upgrade.VendingOneRound)
        {
            temporaryVendingUpgrades.Add(upgrade);
        }

        RefreshTempUpgradeImages();
    }

    public void NotEnoughMoney()
    {
        ShowUpgradeText("Not enough money. You need 50$ to buy something from the vending machine.");
    }

    public void AlreadyUsed()
    {
        ShowUpgradeText("You already used this vending machine during this round.");
    }

    private void RemoveExpiredVendingUpgrades()
    {
        foreach (PassiveUpgrades upgrade in temporaryVendingUpgrades)
        {
            ShowUpgradeText($"- {upgrade.itemName}");
        }

        temporaryVendingUpgrades.Clear();

        RefreshTempUpgradeImages();
    }

    private void ShowUpgradeText(string message)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeUpgradeText(message));
    }

    private IEnumerator FadeUpgradeText(string message)
    {
        upgradeText.text = message;

        upgradeText.alpha = 1f;

        yield return new WaitForSeconds(visibleDuration);

        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            upgradeText.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            yield return null;
        }

        upgradeText.alpha = 0f;
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
    public void CritChanceUp(float effectAmount)
    {
        player.critChance = player.critChance + effectAmount;
    }

    public void CritEffectUp(float effectAmount)
    {
        player.critEffect = player.critEffect + effectAmount;
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

    public void CurrencyIncrease(int effectAmount)
    {
        currency.currencyIncrease = currency.currencyIncrease + effectAmount;
    }

    public void FreeMoney(int effectAmount)
    {
        currency.currency = currency.currency + effectAmount;
    }

    public void KnockbackUp(float effectAmount)
    {
        player.knockbackStrength = player.knockbackStrength + effectAmount;
        player.UpdateStats();
    }

    public void EnemyStrengthUp(int effectAmountInt, float effectAmountFloat)
    {
        waveManager._waveStrength = waveManager._waveStrength + effectAmountFloat;
        waveManager.swarmerCurrencyReward = waveManager.swarmerCurrencyReward * effectAmountInt;
        waveManager.mortarCurrencyReward = waveManager.mortarCurrencyReward * effectAmountInt;
        waveManager.tankCurrencyReward = waveManager.tankCurrencyReward * effectAmountInt;
        waveManager.swarmerEXPReward = waveManager.swarmerEXPReward * effectAmountFloat;
        waveManager.mortarEXPReward = waveManager.mortarEXPReward * effectAmountFloat;
        waveManager.tankEXPReward = waveManager.tankEXPReward * effectAmountFloat;
    }
}