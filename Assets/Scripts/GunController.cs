using System.Collections;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;
    public Transform muzzlePoint;
    public LineRenderer tracer;

    [Header("Gun Settings")]
    public float range = 100f;
    public float tracerDuration = 0.1f;

    private void Start()
    {
        tracer.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f));

        RaycastHit hit;

        Vector3 endPoint;

        if (Physics.Raycast(ray, out hit, range))
        {
            endPoint = hit.point;

            Debug.Log("명중 : " + hit.collider.name);
        }
        else
        {
            endPoint = ray.origin + ray.direction * range;
        }

        StartCoroutine(ShowTracer(endPoint));
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