using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SurvivalShooter.SaveSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MainMenuManager : MonoBehaviour
{
    [Header("Buttons")]
    public Button startButton;
    public Button continueButton;
    public Button exitButton;

    [Header("Scenes")]
    public string gameSceneName = "Level 01";

    ISaveSystem saveSystem;

    void Awake()
    {
        saveSystem = new JsonSaveSystem();
    }

    void Start()
    {
        
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(saveSystem.SaveExists);
        }
            
    }

    public void OnStartNewGame()
    {
        saveSystem.Delete();
        GameSession.LoadRequested = false;
        LoadGame();
    }

    public void OnContinue()
    {
        if (!saveSystem.SaveExists) return;   //jic
        GameSession.LoadRequested = true;
        LoadGame();
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void LoadGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}