using UnityEngine;

[System.Serializable]
public class WeaponData
{
    [Header("Info")]
    public string weaponName = "Pistol";

    [Header("Ammo")]
    public int maxAmmo = 7;

    [Header("Reload")]
    public float reloadTime = 2f;

    [Header("Fire")]
    public float fireCooldown = 0.2f;
    public float range = 100f;

    [Header("Tracer")]
    public float tracerDuration = 0.1f;

    [Header("Recoil")]
    public float cameraRecoil = 1.5f;
    public float weaponRecoil = 3f;

    [Header("ADS")]
    public float adsFOV = 40f;

    public Vector3 hipPosition =
        new Vector3(0.3f, -0.3f, 0.6f);

    public Vector3 adsPosition =
        new Vector3(0f, -0.1f, 0.4f);
}