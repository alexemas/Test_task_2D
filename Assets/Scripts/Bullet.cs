using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // Урон пули
    [SerializeField] private float lifespan = 2f; // Время жизни пули (в секундах)

    private void Start()
    {
        // Уничтожить пулю после заданного времени
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // Если пуля столкнулась с врагом, вызываем функцию, которая может обработать урон врагу
            // Например, вы можете вызвать функцию TakeDamage() у скрипта врага, передав урон как параметр
            // other.GetComponent<EnemyScript>().TakeDamage(damage);
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
            // Затем уничтожаем пулю
            Destroy(gameObject);
        }
    }
}
