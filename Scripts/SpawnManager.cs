using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Game objects
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private GameObject _enemyContainer;
    [SerializeField] private GameObject[] powerups;

    // Powerup
    private float _powerupSpawnTime = 0;

    // Death?
    private bool _stopSpawning = false;
    private bool _end = false;

    public void StartSpawning() {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    void Start() {
        StartSpawning();
    }

    IEnumerator SpawnEnemyRoutine() {
        yield return new WaitForSeconds(3);
        while(!_stopSpawning) {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 7.75f, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5);
        }
    }

    IEnumerator SpawnPowerupRoutine() {
        yield return new WaitForSeconds(3);
        while (!_stopSpawning) {
            Vector3 spawnPosition = new Vector3(Random.Range(-10f, 10f), 7.75f, 0);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(powerups[randomPowerup], spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    IEnumerator SpawnFreeze() {
        yield return new WaitForSeconds(3);
        _stopSpawning = false;
        StartSpawning();
    }

    public void OnPlayerDeath(int livesRemaining) {
        _stopSpawning = true;
        if (livesRemaining > 0) {
            StartCoroutine(SpawnFreeze());
        } else {
            _end = true;
        }
    }
}
