using System.Collections.Generic;
using UnityEngine;

public class UpgradeVendingMachine : MonoBehaviour
{
    [SerializeField] private int cost = 50;

    [SerializeField]
    private List<PassiveUpgrades> possibleUpgrades = new();

    private bool usedThisRound;

    private PlayerCurrency playerCurrency;
    private PassiveUpgradeManager upgradeManager;
    private WaveManager waveManager;

    private float lastWave;

    private void Start()
    {
        playerCurrency = FindFirstObjectByType<PlayerCurrency>();
        upgradeManager = FindFirstObjectByType<PassiveUpgradeManager>();
        waveManager = FindFirstObjectByType<WaveManager>();

        lastWave = waveManager.CurrentWave;
    }

    private void Update()
    {
        if (waveManager.CurrentWave > lastWave)
        {
            usedThisRound = false;
            lastWave = waveManager.CurrentWave;
        }
    }

    public void PurchaseUpgrade()
    {
        if (usedThisRound)
        {
            Debug.Log("Already used this vending machine this round.");
            return;
        }

        if (playerCurrency.currency < cost)
        {
            Debug.Log("Not enough money.");
            return;
        }

        if (possibleUpgrades.Count == 0)
        {
            Debug.Log("No upgrades assigned.");
            return;
        }

        playerCurrency.currency -= cost;

        PassiveUpgrades chosenUpgrade =
            possibleUpgrades[Random.Range(0, possibleUpgrades.Count)];

        upgradeManager.ApplyTemporaryUpgrade(chosenUpgrade);

        usedThisRound = true;

        Debug.Log("Received: " + chosenUpgrade.itemName);
    }
}