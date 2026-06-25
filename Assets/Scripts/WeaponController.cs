using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponController : MonoBehaviour
{
    private Renderer[] weaponRenderers;

    [Header("Weapon")]
    public WeaponData weaponData;

    [Header("Inventory")]
    public int hotbarSlot = 1;

    [Header("References")]
    public Camera playerCamera;
    public Transform muzzlePoint;
    public LineRenderer tracer;
    public HotbarController hotbar;
    public MouseLook mouseLook;
    public Transform weaponHolder;

    [Header("UI")]
    public TMP_Text ammoText;
    public Image ammoIndicator;
    public Image adsMask;

    [Header("Indicator Colors")]
    public Color loadedColor = Color.green;
    public Color emptyColor = Color.red;
    public Color reloadingColor =
        new Color(1f, 0.5f, 0f);

    [Header("ADS")]
    public float normalFOV = 60f;
    public float adsSpeed = 10f;
    public float maskAlpha = 0.6f;

    [Header("Recoil")]
    public float recoilRecoverySpeed = 15f;

    private int currentAmmo;
    private bool isReloading;
    private bool isADS;

    private float nextFireTime;
    private float currentWeaponRecoil;

    private Coroutine reloadCoroutine;

    private bool wasSelected;

    void Start()
    {
        currentAmmo =
            weaponData.maxAmmo;

        tracer.enabled = false;

        UpdateAmmoUI();

        if (adsMask != null)
        {
            Color c = adsMask.color;
            c.a = 0f;
            adsMask.color = c;
        }

        wasSelected = false;

        weaponRenderers =
            GetComponentsInChildren<Renderer>(true);
    }

    void Update()
    {
        bool selected =
            hotbar != null &&
            hotbar.IsSlotSelected(hotbarSlot);

        if (wasSelected && !selected)
        {
            CancelReload();
            CancelADS();
        }

        wasSelected = selected;

        UpdateWeaponUI(selected);

        UpdateWeaponVisibility(selected);

        isADS =
            selected &&
            Input.GetMouseButton(1) &&
            !isReloading;

        UpdateADS();

        if (selected &&
            Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading &&
                currentAmmo <
                weaponData.maxAmmo)
            {
                reloadCoroutine =
                    StartCoroutine(Reload());
            }
        }

        if (selected &&
            Input.GetMouseButton(0) &&
            Time.time >= nextFireTime &&
            !isReloading &&
            currentAmmo > 0)
        {
            Shoot();

            nextFireTime =
                Time.time +
                weaponData.fireCooldown;
        }

        UpdateRecoil();
    }

    void Shoot()
    {
        currentAmmo--;

        UpdateAmmoUI();

        Ray ray =
            playerCamera.ViewportPointToRay(
                new Vector3(
                    0.5f,
                    0.5f,
                    0f));

        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(
            ray,
            out hit,
            weaponData.range))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint =
                ray.origin +
                ray.direction *
                weaponData.range;
        }

        mouseLook.AddRecoil(
            weaponData.cameraRecoil);

        currentWeaponRecoil +=
            weaponData.weaponRecoil;

        StartCoroutine(
            ShowTracer(endPoint));
    }

    IEnumerator Reload()
    {
        isReloading = true;

        UpdateAmmoIndicator();

        yield return new WaitForSeconds(
            weaponData.reloadTime);

        currentAmmo =
            weaponData.maxAmmo;

        isReloading = false;

        reloadCoroutine = null;

        UpdateAmmoUI();
    }

    void CancelReload()
    {
        if (!isReloading)
            return;

        if (reloadCoroutine != null)
        {
            StopCoroutine(
                reloadCoroutine);
        }

        reloadCoroutine = null;

        isReloading = false;

        UpdateAmmoIndicator();
    }

    void CancelADS()
    {
        isADS = false;
    }

    void UpdateADS()
    {
        Vector3 targetPos =
            isADS
            ? weaponData.adsPosition
            : weaponData.hipPosition;

        weaponHolder.localPosition =
            Vector3.Lerp(
                weaponHolder.localPosition,
                targetPos,
                adsSpeed *
                Time.deltaTime);

        float targetFOV =
            isADS
            ? weaponData.adsFOV
            : normalFOV;

        playerCamera.fieldOfView =
            Mathf.Lerp(
                playerCamera.fieldOfView,
                targetFOV,
                adsSpeed *
                Time.deltaTime);

        if (adsMask != null)
        {
            float targetAlpha =
                isADS
                ? maskAlpha
                : 0f;

            Color c =
                adsMask.color;

            c.a =
                Mathf.Lerp(
                    c.a,
                    targetAlpha,
                    adsSpeed *
                    Time.deltaTime);

            adsMask.color = c;
        }
    }

    void UpdateWeaponUI(bool selected)
    {
        if (ammoText != null)
            ammoText.gameObject
                .SetActive(selected);

        if (ammoIndicator != null)
            ammoIndicator.gameObject
                .SetActive(selected);
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text =
                currentAmmo +
                " / " +
                weaponData.maxAmmo;
        }

        UpdateAmmoIndicator();
    }

    void UpdateAmmoIndicator()
    {
        if (ammoIndicator == null)
            return;

        if (isReloading)
            ammoIndicator.color =
                reloadingColor;
        else if (currentAmmo > 0)
            ammoIndicator.color =
                loadedColor;
        else
            ammoIndicator.color =
                emptyColor;
    }

    void UpdateRecoil()
    {
        currentWeaponRecoil =
            Mathf.Lerp(
                currentWeaponRecoil,
                0f,
                recoilRecoverySpeed *
                Time.deltaTime);

        weaponHolder.localRotation =
            Quaternion.Euler(
                -currentWeaponRecoil,
                0f,
                0f);
    }

    void UpdateWeaponVisibility(bool selected)
    {
    foreach (Renderer r in weaponRenderers)
        {
            r.enabled = selected;
        }

    if (tracer != null)
        {
            tracer.enabled = false;
        }
    }

    IEnumerator ShowTracer(
        Vector3 hitPoint)
    {
        tracer.enabled = true;

        tracer.SetPosition(
            0,
            muzzlePoint.position);

        tracer.SetPosition(
            1,
            hitPoint);

        yield return new WaitForSeconds(
            weaponData.tracerDuration);

        tracer.enabled = false;
    }
}