using System;
using System.IO;
using UnityEngine;
using UnityEngine.Audio;
using SurvivalShooter.SaveSystem;


[Serializable]
public class ConfigData
{
    public int version = SaveConstants.CurrentConfigVersion;
    public bool audioEnabled = true;    
    public float musicVolume = 0f;       
    public float effectsVolume = 0f;     
}

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    [Header("Mixers")]
    public AudioMixer masterMixer;      

    [Header("param names")]
    public string musicParam = "musicVol";

    public string effectsParam = "sfxVol";

    public AudioMixer effectsParamMixer;

    public float saveDebounce = 0.3f;

    ConfigData config;
    string path;

    public ConfigData Config => config;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        path = Path.Combine(Application.persistentDataPath, SaveConstants.ConfigFileName);
        Load();
        Apply();
    }

    void Load()
    {
        config = null;
        if (File.Exists(path))
        {
            try { config = JsonUtility.FromJson<ConfigData>(File.ReadAllText(path)); }
            catch (Exception e) { Debug.LogWarning($"[Config] Could not read config: {e.Message}"); }
        }
        if (config == null) config = new ConfigData(); // first run / corrupt -> defaults
    }

    public void Apply()
    {
        if (masterMixer != null) masterMixer.SetFloat(musicParam, config.musicVolume);

        AudioMixer em = effectsParamMixer != null ? effectsParamMixer : masterMixer;
        if (em != null) em.SetFloat(effectsParam, config.effectsVolume);

        AudioListener.volume = config.audioEnabled ? 1f : 0f;
    }

    public void SetMusic(float dB)
    {
        config.musicVolume = dB;
        if (masterMixer != null) masterMixer.SetFloat(musicParam, dB);
        SaveDeferred();
    }

    public void SetEffects(float dB)
    {
        config.effectsVolume = dB;
        AudioMixer em = effectsParamMixer != null ? effectsParamMixer : masterMixer;
        if (em != null) em.SetFloat(effectsParam, dB);
        SaveDeferred();
    }

    public void SetAudioEnabled(bool on)
    {
        config.audioEnabled = on;
        AudioListener.volume = on ? 1f : 0f;
        SaveDeferred();
    }


    void SaveDeferred()
    {
        CancelInvoke(nameof(SaveNow));
        Invoke(nameof(SaveNow), saveDebounce);
    }

    public void SaveNow()
    {
        try { AtomicFile.WriteText(path, JsonUtility.ToJson(config, true)); }
        catch (Exception e) { Debug.LogError($"[Config] Could not write config: {e.Message}"); }
    }

    void OnApplicationQuit()
    {
        CancelInvoke(nameof(SaveNow));
        SaveNow();
    }
}