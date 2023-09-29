using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // ���� ����
    [SerializeField] private float lifespan = 2f; // ����� ����� ���� (� ��������)

    private void Start()
    {
        // ���������� ���� ����� ��������� �������
        Destroy(gameObject, lifespan);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            // ���� ���� ����������� � ������, �������� �������, ������� ����� ���������� ���� �����
            // ��������, �� ������ ������� ������� TakeDamage() � ������� �����, ������� ���� ��� ��������
            // other.GetComponent<EnemyScript>().TakeDamage(damage);
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            enemyHealth.TakeDamage(damage);
            // ����� ���������� ����
            Destroy(gameObject);
        }
    }
}
