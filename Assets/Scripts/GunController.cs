using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GunController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform muzzlePoint;
    public LineRenderer tracer;

    public Transform weaponHolder;
    public MouseLook mouseLook;

    [Header("UI")]
    public TMP_Text ammoText;
    public Image ammoIndicator;
    public Image adsMask;

    [Header("Gun Settings")]
    public float range = 100f;
    public float tracerDuration = 0.1f;

    [Header("Ammo")]
    public int maxAmmo = 7;
    private int currentAmmo;

    [Header("Ammo Indicator")]
    public Color loadedColor = Color.green;
    public Color emptyColor = Color.red;
    public Color reloadingColor = new Color(1f, 0.5f, 0f);

    [Header("Reload")]
    public float reloadTime = 2f;
    private bool isReloading;

    [Header("Fire Rate")]
    public float fireCooldown = 0.2f;

    [Header("Recoil")]
    public float cameraRecoilAmount = 1.5f;
    public float weaponRecoilAmount = 3f;
    public float recoilRecoverySpeed = 15f;

    [Header("ADS")]
    public Vector3 hipPosition = new Vector3(0.3f, -0.3f, 0.6f);
    public Vector3 adsPosition = new Vector3(0f, -0.1f, 0.4f);

    public float normalFOV = 60f;
    public float adsFOV = 40f;

    public float adsSpeed = 10f;
    public float maskAlpha = 0.6f;

    private float nextFireTime;
    private float currentWeaponRecoil;
    private bool isADS;

    void Start()
    {
        tracer.enabled = false;

        currentAmmo = maxAmmo;

        UpdateAmmoUI();
        UpdateAmmoIndicator();

        playerCamera.fieldOfView = normalFOV;

        if (adsMask != null)
        {
            Color c = adsMask.color;
            c.a = 0f;
            adsMask.color = c;
        }
    }

    void Update()
    {
        isADS =
            Input.GetMouseButton(1)
            && !isReloading;

        UpdateADS();

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!isReloading &&
                currentAmmo < maxAmmo &&
                !isADS)
            {
                StartCoroutine(Reload());
            }
        }

        if (Input.GetMouseButton(0)
            && Time.time >= nextFireTime
            && !isReloading
            && currentAmmo > 0)
        {
            Shoot();

            nextFireTime =
                Time.time + fireCooldown;
        }

        UpdateRecoil();
    }

    void UpdateADS()
    {
        Vector3 targetPos =
            isADS ? adsPosition : hipPosition;

        weaponHolder.localPosition =
            Vector3.Lerp(
                weaponHolder.localPosition,
                targetPos,
                adsSpeed * Time.deltaTime);

        float targetFOV =
            isADS ? adsFOV : normalFOV;

        playerCamera.fieldOfView =
            Mathf.Lerp(
                playerCamera.fieldOfView,
                targetFOV,
                adsSpeed * Time.deltaTime);

        if (adsMask != null)
        {
            float targetAlpha =
                isADS ? maskAlpha : 0f;

            Color c = adsMask.color;

            c.a = Mathf.Lerp(
                c.a,
                targetAlpha,
                adsSpeed * Time.deltaTime);

            adsMask.color = c;
        }
    }

    void Shoot()
    {
        currentAmmo--;
        UpdateAmmoUI();

        Ray ray =
            playerCamera.ViewportPointToRay(
                new Vector3(0.5f, 0.5f, 0f));

        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;
        }
        else
        {
            endPoint =
                ray.origin +
                ray.direction * range;
        }

        mouseLook.AddRecoil(cameraRecoilAmount);

        currentWeaponRecoil += weaponRecoilAmount;

        StartCoroutine(ShowTracer(endPoint));
    }

    IEnumerator Reload()
    {
        isReloading = true;

        UpdateAmmoIndicator();

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;

        isReloading = false;

        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text =
                currentAmmo +
                " / " +
                maxAmmo;
        }

        UpdateAmmoIndicator();
    }

    void UpdateAmmoIndicator()
    {
    if (ammoIndicator == null)
        return;

    if (isReloading)
        {
        ammoIndicator.color = reloadingColor;
        }
    else if (currentAmmo > 0)
        {
        ammoIndicator.color = loadedColor;
        }
    else
        {
        ammoIndicator.color = emptyColor;
        }
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

    IEnumerator ShowTracer(Vector3 hitPoint)
    {
        tracer.enabled = true;

        tracer.SetPosition(0, muzzlePoint.position);
        tracer.SetPosition(1, hitPoint);

        yield return new WaitForSeconds(tracerDuration);

        tracer.enabled = false;
    }
}