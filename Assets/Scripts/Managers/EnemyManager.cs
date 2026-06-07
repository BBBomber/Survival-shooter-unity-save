using System;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;

    public string spawnerId = "";

    float timer;               
    bool spawningEnabled = true;

    public string SpawnerId => spawnerId;
    public bool SpawningEnabled => spawningEnabled;
    public float TimeUntilNextSpawn => Mathf.Max(0f, spawnTime - timer);

    void OnEnable() { GameEvents.PlayerDied += StopSpawning; }
    void OnDisable() { GameEvents.PlayerDied -= StopSpawning; }

    void StopSpawning() => spawningEnabled = false;

    void Update()
    {
        if (!spawningEnabled) return;

        timer += Time.deltaTime;
        if (timer >= spawnTime)
        {
            timer -= spawnTime;
            Spawn();
        }
    }

    void Spawn()
    {
        if (enemy == null || spawnPoints == null || spawnPoints.Length == 0) return;
        int i = UnityEngine.Random.Range(0, spawnPoints.Length);
        Instantiate(enemy, spawnPoints[i].position, spawnPoints[i].rotation);
    }

    public void RestoreSpawnerState(bool enabledState, float timeUntilNext)
    {
        spawningEnabled = enabledState;
        timer = Mathf.Clamp(spawnTime - Mathf.Max(0f, timeUntilNext), 0f, spawnTime);
    }
}