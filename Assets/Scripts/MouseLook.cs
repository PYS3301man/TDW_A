using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 150f;

    public Transform playerBody;

    float xRotation = 0f;

    [Header("Recoil")]
    public float recoilAmount = 0f;
    public float recoilRecoverySpeed = 10f;

    private float currentRecoil = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX =
            Input.GetAxis("Mouse X")
            * mouseSensitivity
            * Time.deltaTime;

        float mouseY =
            Input.GetAxis("Mouse Y")
            * mouseSensitivity
            * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        currentRecoil = Mathf.Lerp(
            currentRecoil,
            0f,
            recoilRecoverySpeed * Time.deltaTime
        );

        transform.localRotation =
            Quaternion.Euler(
                xRotation - currentRecoil,
                0f,
                0f
            );

        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void AddRecoil(float amount)
    {
        currentRecoil += amount;
    }
}