using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    bool playerAlive = true;
    bool enemyAlive = true;
    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealth>();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        GameEvents.PlayerDied += HandlePlayerDied;
        if (enemyHealth != null) enemyHealth.Died += HandleEnemyDied;
    }

    void OnDisable()
    {
        GameEvents.PlayerDied -= HandlePlayerDied;
        if (enemyHealth != null) enemyHealth.Died -= HandleEnemyDied;
    }

    void HandlePlayerDied()
    {
        playerAlive = false;
        anim.SetTrigger("PlayerDead");
    }

    void HandleEnemyDied() => enemyAlive = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player) playerInRange = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player) playerInRange = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeBetweenAttacks && playerInRange && enemyAlive && playerAlive)
        {
            Attack();
        }
    }

    void Attack()
    {
        timer = 0f;
        if (playerAlive)
        {
            playerHealth.TakeDamage(attackDamage);
        }
    }
}