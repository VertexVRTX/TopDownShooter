using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickUpType { Health, Ammo, Shield }
    public PickUpType type;

    public float healAmount = 25f;
    public int ammoAmount = 15;
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == PickUpType.Health)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.Heal(healAmount);
                    Destroy(gameObject);
                }
            }
            else if (type == PickUpType.Ammo)
            {
                PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

                if (playerShooting != null)
                {
                    playerShooting.ReloadAmmo(ammoAmount);
                    if (playerHealth != null) playerHealth.PlayAmmoSound();
                    Destroy(gameObject);
                }
            }
            else if (type == PickUpType.Shield)
            {
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.ActivateShield(8f);
                    Destroy(gameObject);
                }
            }
        }
    }
}
