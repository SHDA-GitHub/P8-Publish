using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] _enemySpawners;
    [SerializeField] private float CurrentWave;
    [SerializeField] private float _waveTimer;
    [SerializeField] private float _spawnTimer;
    [SerializeField] private byte _maxEnemies;
    public List<GameObject> CurrentEnemies = new List<GameObject>();

    [Header("Wave data")]
    [SerializeField] private bool _waveCooldown = false;
    [SerializeField] private bool _spawnCooldown = false;

    [Header("UI")]
    [SerializeField] private TMP_Text waveUI;


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
        GameObject enemy = Instantiate(enemyPrefab, targetPos.position, Quaternion.identity);
        enemy.GetComponent<EnemyHealth>().maxHealth *= (1 + CurrentWave);
        enemy.GetComponent<TouchDamage>()._attackDamage *= (1 + CurrentWave);
    }

    private IEnumerator WaveTimer()
    {
        _waveCooldown = true;
        yield return new WaitForSeconds(_waveTimer);
        CurrentWave++;
        waveUI.text = "Wave: " + CurrentWave;
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