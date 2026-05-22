using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private byte _spawnLimit;

    [Header("Wave data")]
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField] private float _enemyScaling;

    [Header("UI")]
    [SerializeField] private GameObject waveUI;
    [SerializeField] private GameObject enemyCountUI;

    private bool _waveCooldown = true;
    private bool _spawnCooldown = true;

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
                StartCoroutine(SpawnTimer());
            }
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        enemy.GetComponent<EnemyHealth>().health *= 1 + _enemyScaling;
        enemy.GetComponent<TouchDamage>()._attackDamage *= 1 + _enemyScaling;
        spawnedEnemies.Add(enemy);
    }

    private IEnumerator WaveTimer()
    {
        _waveCooldown = true;
        yield return new WaitForSeconds(_waveInterval);
        CurrentWave++;
        _waveCooldown = false;
    }

    private IEnumerator SpawnTimer()
    {
        _spawnCooldown = true;
        SpawnEnemy();
        yield return new WaitForSeconds(_spawnInterval);
        _spawnCooldown = false;
    }
}