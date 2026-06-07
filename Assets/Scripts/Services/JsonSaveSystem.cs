using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SurvivalShooter.SaveSystem
{
    
    public class JsonSaveSystem : ISaveSystem
    {
        readonly string path;
        readonly string backupPath;

        public JsonSaveSystem()
        {
            path = Path.Combine(Application.persistentDataPath, SaveConstants.SaveFileName);
            backupPath = path + ".bak";
        }

        public bool SaveExists => File.Exists(path) || File.Exists(backupPath);

        public void Save(SaveData data)
        {
            if (data == null) return;
            data.version = SaveConstants.CurrentSaveVersion;
            string json = JsonUtility.ToJson(data, true);
            AtomicFile.WriteText(path, json);
        }

        public SaveData Load()
        {
            SaveData data = TryLoadFrom(path) ?? TryLoadFrom(backupPath);
            if (data == null) return null;
            return SaveMigration.Migrate(data);
        }

        SaveData TryLoadFrom(string p)
        {
            if (!File.Exists(p)) return null;
            try
            {
                string json = File.ReadAllText(p);
                return JsonUtility.FromJson<SaveData>(json); 
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[SaveSystem] Could not read save at '{p}': {e.Message}");
                return null;
            }
        }

        public void Delete()
        {
            TryDelete(path);
            TryDelete(backupPath);
            TryDelete(path + ".tmp");
        }

        static void TryDelete(string p)
        {
            try { if (File.Exists(p)) File.Delete(p); }
            catch (Exception e) { Debug.LogWarning($"[SaveSystem] Could not delete '{p}': {e.Message}"); }
        }
    }
}