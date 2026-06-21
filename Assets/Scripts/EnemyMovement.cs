using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f;

    private Transform target;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        GameObject baseObject = GameObject.Find("Base");

        if (baseObject != null)
        {
            target = baseObject.transform;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction =
            (target.position - transform.position).normalized;

        direction.y = 0;

        rb.MovePosition(
            rb.position +
            direction * speed * Time.fixedDeltaTime
        );
    }
}