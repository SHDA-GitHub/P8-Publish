using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;

public class EnemyHealth : BaseEnemy
{
    [SerializeField] private Image healthBarFill;
    private Camera _mainCam;
    private PlayerEXP playerEXP;
    private PlayerCurrency playerCurrency;
    public float health;
    public float maxHealth;
    [SerializeField] private float EXPReward;
    [SerializeField] private int CurrencyReward;

    private WaveManager waveManager;
    private void Awake()
    {
        waveManager = FindFirstObjectByType<WaveManager>();
        waveManager.CurrentEnemies.Add(gameObject);
    }
    void Start()
    {
        health = maxHealth;
        playerEXP = FindFirstObjectByType<PlayerEXP>();
        playerCurrency = FindFirstObjectByType<PlayerCurrency>();
    }
    private void Update()
    {
        if (health <= 0)
        {
            Death();
        }
        healthBarFill.fillAmount = health / maxHealth;
        
    }

    protected override void Death()
    {
        waveManager.CurrentEnemies.Remove(gameObject);
        playerCurrency.kills += 1;
        playerCurrency.AddEXP(CurrencyReward);
        playerEXP.AddEXP(EXPReward);
        Debug.Log("Enemy Dies");
        base.Death();
    }
}