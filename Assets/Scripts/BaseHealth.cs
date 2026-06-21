using UnityEngine;

public class BaseHealth : MonoBehaviour
{
    public int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log("Base HP : " + health);

        if (health <= 0)
        {
            Debug.Log("게임 오버!");
        }
    }
}