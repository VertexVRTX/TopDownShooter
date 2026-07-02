using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float health = 20f;
    public float explosionRadius = 5f;
    public float explosionDamage = 50f;
    public GameObject explosionVFX;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (explosionVFX != null)
        {
            Instantiate(explosionVFX, transform.position, Quaternion.identity);
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider nearbyObject in colliders)
        {
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(explosionDamage);
            }

            PlayerHealth player = nearbyObject.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(explosionDamage);
            }
        }

        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
