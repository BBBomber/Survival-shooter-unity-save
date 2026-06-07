using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        GameEvents.PlayerDied += ShowGameOver;
    }

    void OnDisable()
    {
        GameEvents.PlayerDied -= ShowGameOver;
    }

    void ShowGameOver()
    {
        anim.SetTrigger("GameOver");
    }
}