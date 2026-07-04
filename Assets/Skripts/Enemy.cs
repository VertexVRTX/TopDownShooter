using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;

    public float damage = 15f;
    public float attackCooldown = 1f;
    private float nextAttackTime;

    [Header("Attack Distance")]
    public float attackRange = 0.5f;

    private Transform playerTransform;
    private NavMeshAgent agent;
    public System.Action OnEnemyDestroyed;

    [Header("Monster Audio")]
    public AudioSource enemyAudioSource;
    public AudioClip[] growlSounds;
    public float minGrowlInterval = 4f;
    public float maxGrowlInterval = 8f;
    private float nextGrowlTime;

    [Header("Effects")]
    public GameObject deathVFX;

    void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        if (enemyAudioSource == null) enemyAudioSource = GetComponent<AudioSource>();
        nextGrowlTime = Time.time + Random.Range(1f, maxGrowlInterval);
    }

    void Update()
    {
        if (playerTransform != null && agent.enabled)
        {
            agent.SetDestination(playerTransform.position);

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
            {
                PlayerHealth playerHealth = playerTransform.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    nextAttackTime = Time.time + attackCooldown;
                }
            }
        }

        if (Time.time >= nextGrowlTime)
        {
            PlayGrowl();
            nextGrowlTime = Time.time + Random.Range(minGrowlInterval, maxGrowlInterval);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (deathVFX != null)
        {
            Instantiate(deathVFX, transform.position, transform.rotation);
        }

        if (UIManager.Instance != null)
        {
            UIManager.Instance.AddScore(10);
        }

        if (OnEnemyDestroyed != null) OnEnemyDestroyed.Invoke();
        Destroy(gameObject);
    }

    void PlayGrowl()
    {
        if (enemyAudioSource != null && growlSounds.Length > 0)
        {
            AudioClip randomClip = growlSounds[Random.Range(0, growlSounds.Length)];
            enemyAudioSource.PlayOneShot(randomClip);
        }
    }
}
