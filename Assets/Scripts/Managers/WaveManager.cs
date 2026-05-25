using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] _enemySpawners;
    [SerializeField] private float CurrentWave;
    [SerializeField] private float _waveInterval;
    [SerializeField] private float _spawnInterval;
    [SerializeField] private byte _maxEnemies;

    [Header("Wave data")]
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField] private bool _waveCooldown = false;
    [SerializeField] private bool _spawnCooldown = false;

    [Header("UI")]
    [SerializeField] private GameObject waveUI;
    [SerializeField] private GameObject enemyCountUI;


    private void Update()
    {
        if(!_waveCooldown)
        {
            StartCoroutine(WaveTimer());
        }

        if (_spawnCooldown)
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (spawnedEnemies[i] == null)
                {
                    spawnedEnemies.RemoveAt(i);
                }
            }
            print("help");

        }else if (!_spawnCooldown && spawnedEnemies.Count <= _maxEnemies)
        {
            print("spawning");
            for (int i = 0; i < _enemySpawners.Length; i++)
            {
                StartCoroutine(SpawnTimer(_enemySpawners[i]));
            }
        }
    }

    private void SpawnEnemy(Transform targetPos)
    {
        GameObject enemy = Instantiate(enemyPrefab, targetPos.position, Quaternion.identity);
        spawnedEnemies.Add(enemy);
        enemy.GetComponent<EnemyHealth>().maxHealth *= (1 + CurrentWave);
        enemy.GetComponent<TouchDamage>()._attackDamage *= (1 + CurrentWave);
    }

    private IEnumerator WaveTimer()
    {
        _waveCooldown = true;
        yield return new WaitForSeconds(_waveInterval);
        CurrentWave++;
        _waveCooldown = false;
    }

    private IEnumerator SpawnTimer(Transform targetPos)
    {
        _spawnCooldown = true;
        SpawnEnemy(targetPos);
        yield return new WaitForSeconds(_spawnInterval);
        _spawnCooldown = false;
    }
}