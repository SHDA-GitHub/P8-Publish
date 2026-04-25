using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private byte spawnInterval;

    public List<GameObject> spawnedEnemies = new List<GameObject>();
    [SerializeField] private byte spawnLimit = 5;

    private bool spawnCooldown;
    private void Update()
    {
        if (spawnCooldown || spawnedEnemies.Count >= spawnLimit)
        {
            for (int i = 0; i < spawnedEnemies.Count; i++)
            {
                if (spawnedEnemies[i] == null)
                {
                    spawnedEnemies.RemoveAt(i);
                }
                if (i == spawnLimit)
                {
                    i = 0;
                }
            }
            return;
        }
        StartCoroutine(SpawnTimer());
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        spawnedEnemies.Add(enemy);
    }
    
    private IEnumerator SpawnTimer()
    {
        spawnCooldown = true;
        SpawnEnemy();
        yield return new WaitForSeconds(spawnInterval);
        spawnCooldown = false;
    }
}
