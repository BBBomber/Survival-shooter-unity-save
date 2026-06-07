using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using SurvivalShooter.SaveSystem;


public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }

    public static bool LoadFreezeActive { get; private set; }

    public string mainMenuSceneName = "MainMenu";

    public EnemyCatalog enemyCatalog; //enemy type to prefab
    public float loadFreezeSeconds = 3f;

    ISaveSystem saveSystem;

    void Awake()
    {
        Instance = this;
        saveSystem = new JsonSaveSystem();
    }

    void Start()
    {

        if (GameSession.LoadRequested)
        {
            GameSession.LoadRequested = false;
            SaveData data = saveSystem.Load();
            if (data != null)
            {
                RestoreState(data);
                StartCoroutine(FreezeThenPlay());
            }
            else Debug.LogWarning("[SaveSystem] No save found. Either user should not have been able to click on continue or something else is wrong");
        }
    }

    void OnEnable() { GameEvents.PlayerDied += OnPlayerDied; }
    void OnDisable() { GameEvents.PlayerDied -= OnPlayerDied; }
    void OnPlayerDied() { saveSystem.Delete(); }


    System.Collections.IEnumerator FreezeThenPlay()
    {
        LoadFreezeActive = true;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(loadFreezeSeconds);
        Time.timeScale = 1f;
        LoadFreezeActive = false;
    }

    public void SaveAndReturnToMenu()
    {
        saveSystem.Save(CaptureState());
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    SaveData CaptureState()
    {
        var data = new SaveData { score = ScoreManager.score };

        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        if (playerGo != null)
        {
            var ph = playerGo.GetComponent<PlayerHealth>();
            data.player.health = ph != null ? ph.CurrentHealth : 0;
            data.player.position = playerGo.transform.position;
            data.player.rotation = playerGo.transform.rotation;
        }

        Camera cam = Camera.main;
        if (cam != null)
        {
            data.camera.position = cam.transform.position;
            data.camera.rotation = cam.transform.rotation;
        }

        foreach (var sp in FindObjectsByType<EnemyManager>(FindObjectsSortMode.None))
        {
            data.spawners.Add(new SpawnerSaveData
            {
                spawnerId = sp.SpawnerId,
                enabled = sp.SpawningEnabled,
                timeUntilNextSpawn = sp.TimeUntilNextSpawn
            });
        }

        foreach (var e in EnemyHealth.LiveEnemies)
        {
            if (e == null) continue;
            var type = e.GetComponent<EnemyType>();
            data.enemies.Add(new EnemySaveData
            {
                enemyTypeId = type != null ? type.id : "",
                health = e.CurrentHealth,
                position = e.transform.position,
                rotation = e.transform.rotation
            });
        }

        return data;
    }

    void RestoreState(SaveData data)
    {
        ScoreManager.score = data.score;

        GameObject playerGo = GameObject.FindGameObjectWithTag("Player");
        if (playerGo != null)
        {
            playerGo.transform.SetPositionAndRotation(data.player.position, data.player.rotation);
            var rb = playerGo.GetComponent<Rigidbody>();
            if (rb != null) { rb.position = data.player.position; rb.rotation = data.player.rotation; }

            var ph = playerGo.GetComponent<PlayerHealth>();
            if (ph != null) ph.RestoreHealth(data.player.health);
        }

        var managers = FindObjectsByType<EnemyManager>(FindObjectsSortMode.None);
        foreach (var s in data.spawners)
        {
            foreach (var m in managers)
            {
                if (m.SpawnerId == s.spawnerId) { m.RestoreSpawnerState(s.enabled, s.timeUntilNextSpawn); break; }
            }

        }

        foreach (var e in new List<EnemyHealth>(EnemyHealth.LiveEnemies))
        {
            if (e != null) Destroy(e.gameObject);
        }

        EnemyHealth.LiveEnemies.Clear();

        if (enemyCatalog == null)
        {
            Debug.LogError("[SaveSystem] No enemy catalog assigned; ");
            return;
        }

        foreach (var es in data.enemies)
        {
            GameObject prefab = enemyCatalog.GetPrefab(es.enemyTypeId);
            if (prefab == null)
            {
                Debug.LogWarning($"[SaveSystem] No enemy type in da catalog. Wyd? '{es.enemyTypeId}'.");
                continue;
            }

            GameObject go = Instantiate(prefab, es.position, es.rotation);
            var eh = go.GetComponent<EnemyHealth>();
            if (eh != null) eh.RestoreHealth(es.health);
        }

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            mainCam.transform.SetPositionAndRotation(data.camera.position, data.camera.rotation);
            var follow = mainCam.GetComponent<CameraFollow>();
            if (follow != null) follow.RecalculateOffset();
        }


    }
}