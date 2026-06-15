using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private List<GameObject> enemyPrefab;
    [SerializeField] private Transform[] _enemySpawners;
    [SerializeField] private float CurrentWave;
    [SerializeField] private float _waveTimer;
    [SerializeField] private float _spawnTimer;
    [SerializeField] private byte _maxEnemies;
    public List<GameObject> CurrentEnemies = new List<GameObject>();

    [Header("Wave data")]
    [SerializeField] private bool _waveCooldown = false;
    [SerializeField] private bool _spawnCooldown = false;
    public float _waveStrength = 0f;

    [Header("UI")]
    [SerializeField] private GameObject waveUI;
    [SerializeField] private GameObject enemyCountUI;

    [Header("SpawnWeight")]
    [SerializeField] private float _mortarSpawnWeight = 1f;
    [SerializeField] private float _tankSpawnWeight = 1f;

    [Header("EnemyRewards")]
    public float swarmerEXPReward = 25f;
    public float mortarEXPReward = 50f;
    public float tankEXPReward = 100f;
    public int swarmerCurrencyReward = 5;
    public int mortarCurrencyReward = 10;
    public int tankCurrencyReward = 15;

    private void Awake()
    {
        if (enemyPrefab == null)
        enemyPrefab = new List<GameObject>();
    }

    private void Update()
    {
        if(!_waveCooldown)
        {
            StartCoroutine(WaveTimer());
        }
        if (!_spawnCooldown)
        {
            StartCoroutine(SpawnTimer());
        }
    }

    private void SpawnEnemy(Transform targetPos)
    {
        float roll = Random.Range(0f, 10f);

        if (roll < _mortarSpawnWeight)
        {
            SpawnMortar(targetPos);
        }
        else if (roll < _mortarSpawnWeight + _tankSpawnWeight)
        {
            SpawnTank(targetPos);
        }
        else
        {
            SpawnSwarmer(targetPos);
        }
    }

    private void SpawnSwarmer(Transform targetPos)
    {
        GameObject enemy = Instantiate(enemyPrefab[0], targetPos.position, Quaternion.identity);
        enemy.GetComponent<EnemyHealth>().maxHealth *= (1 + _waveStrength);
        enemy.GetComponent<TouchDamage>()._attackDamage *= (1 + _waveStrength);
        enemy.GetComponent<EnemyHealth>().CurrencyReward = swarmerCurrencyReward;
        enemy.GetComponent<EnemyHealth>().EXPReward = swarmerEXPReward;
    }

    private void SpawnMortar(Transform targetPos)
    {
        GameObject enemy = Instantiate(enemyPrefab[1], targetPos.position, Quaternion.identity);
        enemy.GetComponent<EnemyHealth>().maxHealth *= (1 + _waveStrength);
        enemy.GetComponent<EnemyHealth>().CurrencyReward = mortarCurrencyReward;
        enemy.GetComponent<EnemyHealth>().EXPReward = mortarEXPReward;
    }

    private void SpawnTank(Transform targetPos)
    {
        GameObject enemy = Instantiate(enemyPrefab[2], targetPos.position, Quaternion.identity);
        enemy.GetComponent<EnemyHealth>().maxHealth *= (1 + _waveStrength);
        enemy.GetComponent<TankEnemy>().meleeDMG *= (1 + _waveStrength);
        enemy.GetComponent<EnemyHealth>().CurrencyReward = tankCurrencyReward;
        enemy.GetComponent<EnemyHealth>().EXPReward = tankEXPReward;
    }

    private IEnumerator WaveTimer()
    {
        _waveCooldown = true;
        yield return new WaitForSeconds(_waveTimer);
        CurrentWave++;
        _waveStrength++;
        _waveCooldown = false;
    }

    private IEnumerator SpawnTimer()
    {
        _spawnCooldown = true;
        for (int i = 0; i < _enemySpawners.Length; i++)
        {
            if(CurrentEnemies.Count >= _maxEnemies)
            {
                break;
            }
            SpawnEnemy(_enemySpawners[i]);
        }
        yield return new WaitForSeconds(_spawnTimer);
        _spawnCooldown = false;
    }
}