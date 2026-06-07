using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Survival Shooter/Enemy Catalog", fileName = "EnemyCatalog")]
public class EnemyCatalog : ScriptableObject
{
    [Serializable]
    public class Entry
    {
        public string id;
        public GameObject prefab;
    }

    public List<Entry> entries = new List<Entry>();

    public GameObject GetPrefab(string id)
    {
        foreach (var e in entries)
        {
            if (e.id == id) return e.prefab;
        }
            
        return null;
    }
}