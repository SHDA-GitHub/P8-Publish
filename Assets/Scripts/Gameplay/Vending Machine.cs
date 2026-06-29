using System.Collections.Generic;
using UnityEngine;

public class UpgradeVendingMachine : MonoBehaviour
{
    [SerializeField] private int cost = 50;

    [SerializeField]
    private List<PassiveUpgrades> possibleUpgrades = new();

    private bool usedThisRound;

    [SerializeField ]private PlayerCurrency playerCurrency;
    [SerializeField] private PassiveUpgradeManager upgradeManager;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private AudioClip purchaseSuccess;
    [SerializeField] private AudioClip purchaseFailure;
    [SerializeField] private AudioSource audioSource;

    private float lastWave;

    private void Start()
    {
        Debug.Log($"{name} upgrades count = {possibleUpgrades.Count}");
        playerCurrency = FindFirstObjectByType<PlayerCurrency>();
        upgradeManager = FindFirstObjectByType<PassiveUpgradeManager>();
        waveManager = FindFirstObjectByType<WaveManager>();
        audioSource = GetComponent<AudioSource>();

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
            audioSource.clip = purchaseFailure;
            audioSource.Play();
            Debug.Log("Already used this vending machine this round.");
            return;
        }

        if (playerCurrency.currency < cost)
        {
            audioSource.clip = purchaseFailure;
            audioSource.Play();
            Debug.Log("Not enough money.");
            return;
        }

        if (possibleUpgrades.Count == 0)
        {
            audioSource.clip = purchaseFailure;
            audioSource.Play();
            Debug.Log("No upgrades assigned.");
            return;
        }

        else
        {
            audioSource.clip = purchaseSuccess;
            audioSource.Play();

            playerCurrency.currency -= cost;

            PassiveUpgrades chosenUpgrade =
                possibleUpgrades[Random.Range(0, possibleUpgrades.Count)];

            upgradeManager.ApplyTemporaryUpgrade(chosenUpgrade);

            usedThisRound = true;

            Debug.Log("Received: " + chosenUpgrade.itemName);
        }
    }
}