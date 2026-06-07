using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace SurvivalShooter.SaveSystem
{
    // STUB
    public static class SaveMigration
    {
        public static SaveData Migrate(SaveData data)
        {
            if (data == null) return null;
            if (data.version == SaveConstants.CurrentSaveVersion) return data;

            //run steps here

            if (data.version != SaveConstants.CurrentSaveVersion)
            {
                Debug.LogWarning($"[SaveSystem] Save version {data.version} differs from current " + $"{SaveConstants.CurrentSaveVersion}. Loading as is.");

                data.version = SaveConstants.CurrentSaveVersion;
            }
            return data;
        }
    }
}