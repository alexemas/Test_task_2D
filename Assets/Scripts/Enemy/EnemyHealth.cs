using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f; // ������������ �������� �����
    private float currentHealth; // ������� �������� �����

    private Image healthBarImage; // ������ �� ����������� ������� �������� �����

    [SerializeField] private GameObject[] deathSpawnObjects; // ������ ��������, ������� ��������� ��� ������ �����

    private EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        currentHealth = maxHealth; // ������������� ��������� �������� ������ �������������
        healthBarImage = FindClosestHealthBarImage(); // ������� ��������� ����������� ������� ��������
        UpdateHealthBar(); // ��������� ������� �������� ��� ������
    }

    // ������� ��� ������ ���������� ����������� ������� ��������
    private Image FindClosestHealthBarImage()
    {
        Image closestHealthBar = null;
        float closestDistance = float.MaxValue;

        GameObject[] healthBars = GameObject.FindGameObjectsWithTag("HealthBar"); // ��������������, ��� ������� �������� ����� ��� "HealthBar"

        foreach (GameObject healthBarObject in healthBars)
        {
            float distance = Vector3.Distance(transform.position, healthBarObject.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHealthBar = healthBarObject.GetComponent<Image>();
            }
        }

        return closestHealthBar;
    }

    // ������� ��� ��������� �����
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        enemyController.isChasingPlayer = true;
        // ���� �������� ������ ��� ����� ����, �� ���� ����
        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthBar(); // ��������� ������� �������� ����� ��������� �����
    }

    // ������� ��� ���������� ������� �������� ���������
    private void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            float healthPercent = currentHealth / maxHealth;
            healthBarImage.fillAmount = healthPercent;
        }
    }

    // ������� ��� ��������� ������ �����
    private void Die()
    {
        // �������� ������� � ������� �����
        Vector3 deathPosition = transform.position;
        Quaternion deathRotation = transform.rotation;

        // ����� ���������� ������� �� ������� ��� ������ �����
        if (deathSpawnObjects != null && deathSpawnObjects.Length > 0)
        {
            int randomIndex = Random.Range(0, deathSpawnObjects.Length); // ���������� ��������� ������
            GameObject randomSpawnObject = deathSpawnObjects[randomIndex]; // �������� ��������� ������
            Instantiate(randomSpawnObject, deathPosition, deathRotation);
        }

        // ��������, ����������� ������� ����� ��� ��������������� �������� ������.
        Destroy(transform.parent.gameObject); // ������� ������: ����������� ������� �����.
    }
}
