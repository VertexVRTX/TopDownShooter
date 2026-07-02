using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public HealthBar healthBar;

    [Header("Shield Settings")]
    public GameObject shieldVisualObject;
    private bool isShieldActive = false;

    [Header("Audio")]
    public AudioSource playerAudioSource;
    public AudioClip damageSound;
    public AudioClip healSound;
    public AudioClip ammoSound;
    public AudioClip shieldSound;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if (shieldVisualObject != null) shieldVisualObject.SetActive(false);

        if (playerAudioSource == null) playerAudioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        if (isShieldActive)
        {
            return;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);

        PlaySound(damageSound);
        if (currentHealth <= 0) Die();
    }

    public void ActivateShield(float duration)
    {
        PlaySound(shieldSound);
        if (!isShieldActive)
        {
            StartCoroutine(ShieldDurationRoutine(duration));
        }
    }

    IEnumerator ShieldDurationRoutine(float duration)
    {
        isShieldActive = true;
        if (shieldVisualObject != null) shieldVisualObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        isShieldActive = false;
        if (shieldVisualObject != null) shieldVisualObject.SetActive(false);
    }

    public void Heal(float amount)
    {
        PlaySound(healSound);
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (healthBar != null) healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void PlayAmmoSound()
    {
        PlaySound(ammoSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (playerAudioSource != null && clip != null)
        {
            playerAudioSource.PlayOneShot(clip);
        }
    }

    void Die() { UIManager.Instance.ShowGameOver(); }
}
