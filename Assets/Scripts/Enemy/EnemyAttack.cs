using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount = 10.0f; // Количество урона, наносимого игроку
    [SerializeField] private float attackInterval = 2.0f; // Интервал между атаками
    private float startAttackInterval;
    private bool isAttacking;

    private void Start()
    {
        startAttackInterval = attackInterval;
    }

    private IEnumerator AttackTimer(PlayerHealth playerHealth)
    {
        if (isAttacking)
        {
            playerHealth.TakeDamage(damageAmount);

            while (startAttackInterval >= 0)
            {
                startAttackInterval -= Time.deltaTime;
                yield return null;
            }
            startAttackInterval = attackInterval;
            StartCoroutine(AttackTimer(playerHealth));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                isAttacking = true;
                // Запустить корутину для таймера
                StartCoroutine(AttackTimer(playerHealth));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isAttacking = false;
        }
    }
}
