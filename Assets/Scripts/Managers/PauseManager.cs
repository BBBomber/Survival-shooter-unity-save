using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using static System.Net.Mime.MediaTypeNames;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{

    public AudioMixerSnapshot paused;
    public AudioMixerSnapshot unpaused;

    public string mainMenuSceneName = "MainMenu";

    public Toggle audioToggle;
    public Slider musicSlider;
    public Slider effectsSlider;

    Canvas canvas;

    void Awake()
    {
        if (ConfigManager.Instance == null) return;
        var cfg = ConfigManager.Instance.Config;

        if (audioToggle != null)
        {
            audioToggle.SetIsOnWithoutNotify(cfg.audioEnabled);
            audioToggle.onValueChanged.AddListener(ConfigManager.Instance.SetAudioEnabled);
        }

        if (musicSlider != null)
        {
            musicSlider.SetValueWithoutNotify(cfg.musicVolume);
            musicSlider.onValueChanged.AddListener(ConfigManager.Instance.SetMusic);
        }

        if (effectsSlider != null)
        {
            effectsSlider.SetValueWithoutNotify(cfg.effectsVolume);
            effectsSlider.onValueChanged.AddListener(ConfigManager.Instance.SetEffects);
        }
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
    }

    void Update()
    {
        if (SaveGameManager.LoadFreezeActive) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            canvas.enabled = !canvas.enabled;
            Pause();
        }
    }

    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
        Lowpass();

    }

    void Lowpass()
    {
        if (Time.timeScale == 0)
        {
            paused.TransitionTo(.01f);
        }

        else

        {
            unpaused.TransitionTo(.01f);
        }
    }

    public void SaveAndExitToMenu()
    {
        if (SaveGameManager.Instance != null)
        {
            SaveGameManager.Instance.SaveAndReturnToMenu();
        }
        else
        {
            Debug.LogWarning("[PauseManager] No SaveManager found; returning to menu.");
            Time.timeScale = 1f;
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}