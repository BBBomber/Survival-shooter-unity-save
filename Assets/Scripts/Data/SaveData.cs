using System;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalShooter.SaveSystem
{

    [Serializable]
    public class SaveData
    {
        public int version = SaveConstants.CurrentSaveVersion;
        public int score;
        public PlayerSaveData player = new PlayerSaveData();
        public CameraSaveData camera = new CameraSaveData();
        public List<SpawnerSaveData> spawners = new List<SpawnerSaveData>();
        public List<EnemySaveData> enemies = new List<EnemySaveData>();
    }

        [Serializable]
    public class PlayerSaveData
    {
        public int health;
        public Vector3 position;
        public Quaternion rotation = Quaternion.identity;
    }

    [Serializable]
    public class CameraSaveData
    {
        public Vector3 position;
        public Quaternion rotation = Quaternion.identity;
    }

    [Serializable]
    public class SpawnerSaveData
    {
        public string spawnerId;
        public bool enabled;
        public float timeUntilNextSpawn;
    }

    [Serializable]
    public class EnemySaveData
    {
        public string enemyTypeId; 
        public int health;
        public Vector3 position;
        public Quaternion rotation = Quaternion.identity;
    }

    public static class SaveConstants
    {
        public const int CurrentSaveVersion = 1;
        public const int CurrentConfigVersion = 1;

        public const string SaveFileName = "savegame.json";
        public const string ConfigFileName = "config.json";
    }
}