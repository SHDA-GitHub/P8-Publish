using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DataCarrying : MonoBehaviour
{
    public static DataCarrying Instance;

    [Header("References")]
    private WaveManager waveManager;
    private Timer timer;
    private PlayerCurrency currency;
    private PlayerEXP exp;
    private PassiveUpgradeManager upgradeManager;

    [Header("Live Stats")]
    public int currentWave;
    public float currentTime;
    public int kills;

    [Header("Run Totals")]
    public float totalEXPGained;
    public int totalCurrencyGained;

    [Header("Max Capped Upgrades Chosen")]
    public Dictionary<string, int> cappedUpgradeChoices = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PlayScene")
        {
            ResetRun();

            waveManager = FindFirstObjectByType<WaveManager>();
            timer = FindFirstObjectByType<Timer>();
            currency = FindFirstObjectByType<PlayerCurrency>();
            exp = FindFirstObjectByType<PlayerEXP>();
            upgradeManager = FindFirstObjectByType<PassiveUpgradeManager>();
        }
    }

    private void Start()
    {
        waveManager = FindFirstObjectByType<WaveManager>();
        timer = FindFirstObjectByType<Timer>();
        currency = FindFirstObjectByType<PlayerCurrency>();
        exp = FindFirstObjectByType<PlayerEXP>();
        upgradeManager = FindFirstObjectByType<PassiveUpgradeManager>();
    }

    private void Update()
    {
        if (waveManager != null)
            currentWave = waveManager.CurrentWave;

        if (timer != null)
            currentTime = timer.currentTime;

        if (currency != null)
            kills = currency.kills;
    }

    private void ResetRun()
    {
        currentWave = 0;
        currentTime = 0;
        kills = 0;
        totalEXPGained = 0;
        totalCurrencyGained = 0;

        cappedUpgradeChoices.Clear();
    }

    public void AddEXPGained(float amount)
    {
        totalEXPGained += amount;
    }

    public void AddCurrencyGained(int amount)
    {
        totalCurrencyGained += amount;
    }

    public void RecordCappedUpgrade(string upgradeName)
    {
        if (!cappedUpgradeChoices.ContainsKey(upgradeName))
            cappedUpgradeChoices.Add(upgradeName, 0);

        cappedUpgradeChoices[upgradeName]++;
    }
}