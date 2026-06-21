using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int damage = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            BaseHealth baseHealth =
                collision.gameObject.GetComponent<BaseHealth>();

            if (baseHealth != null)
            {
                baseHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}