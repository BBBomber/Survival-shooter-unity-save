using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Transform player;
    EnemyHealth enemyHealth;
    UnityEngine.AI.NavMeshAgent nav;
    bool active = true;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    void OnEnable()
    {
        GameEvents.PlayerDied += Stop;
        if (enemyHealth != null) enemyHealth.Died += Stop;
    }

    void OnDisable()
    {
        GameEvents.PlayerDied -= Stop;
        if (enemyHealth != null) enemyHealth.Died -= Stop;
    }

    void Stop()
    {
        active = false;
        if (nav != null) nav.enabled = false;
    }

    void Update()
    {
        if (active && nav.enabled)
        {
            nav.SetDestination(player.position);
        }
    }
}