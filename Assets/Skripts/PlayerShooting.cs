using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public AudioSource shotAudio;

    [Header("Weapon Status")]
    public bool isShotgunUnlocked = false;
    private bool usingShotgun = false;

    [Header("UI Hierarchy Objects")]
    public GameObject rifleUIObject;
    public GameObject shotgunUIObject;

    [Header("Rifle Settings")]
    public int rifleMaxAmmo = 30;
    public float rifleFireRate = 0.15f;
    private int currentRifleAmmo;

    [Header("Shotgun Settings")]
    public int shotgunMaxAmmo = 8;
    public float shotgunFireRate = 0.6f;
    private int currentShotgunAmmo;

    [Header("Notification UI")]
    public TMP_Text notificationText;

    private float nextFireTime = 0f;

    void Start()
    {
        currentRifleAmmo = rifleMaxAmmo;
        currentShotgunAmmo = shotgunMaxAmmo;

        UpdateWeaponUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isShotgunUnlocked)
        {
            usingShotgun = !usingShotgun;
            UpdateWeaponUI();
        }

        if (Time.timeScale == 0f || EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
        {
            int currentAmmo = usingShotgun ? currentShotgunAmmo : currentRifleAmmo;

            if (currentAmmo > 0)
            {
                if (usingShotgun) ShootShotgun();
                else ShootRifle();

                nextFireTime = Time.time + (usingShotgun ? shotgunFireRate : rifleFireRate);
            }
        }
    }

    void ShootRifle()
    {
        currentRifleAmmo--;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (shotAudio != null) shotAudio.Play();
        UpdateAmmoText();
    }

    void ShootShotgun()
    {
        currentShotgunAmmo--;

        float[] angles = { -15f, 0f, 15f };
        foreach (float angle in angles)
        {
            Quaternion rotation = firePoint.rotation * Quaternion.Euler(0, angle, 0);
            Instantiate(bulletPrefab, firePoint.position, rotation);
        }

        if (shotAudio != null) shotAudio.Play();
        UpdateAmmoText();
    }

    public void UnlockShotgun()
    {
        isShotgunUnlocked = true;
        StartCoroutine(ShowNotification("Shotgun unlocked! Press Q to switch."));
    }

    public void ReloadAmmo(int amount)
    {
        if (usingShotgun)
        {
            currentShotgunAmmo = Mathf.Clamp(currentShotgunAmmo + amount, 0, shotgunMaxAmmo);
        }
        else
        {
            currentRifleAmmo = Mathf.Clamp(currentRifleAmmo + amount, 0, rifleMaxAmmo);
        }
        UpdateAmmoText();
    }

    void UpdateWeaponUI()
    {
        if (rifleUIObject != null && shotgunUIObject != null)
        {
            rifleUIObject.SetActive(!usingShotgun);
            shotgunUIObject.SetActive(usingShotgun);
        }
        UpdateAmmoText();
    }

    void UpdateAmmoText()
    {
        int current = usingShotgun ? currentShotgunAmmo : currentRifleAmmo;
        int max = usingShotgun ? shotgunMaxAmmo : rifleMaxAmmo;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdateAmmoUI(current, max);
        }
    }

    IEnumerator ShowNotification(string message)
    {
        if (notificationText == null)
            yield break;

        notificationText.text = message;
        notificationText.gameObject.SetActive(true);

        yield return new WaitForSeconds(10f);

        notificationText.gameObject.SetActive(false);
    }
}
