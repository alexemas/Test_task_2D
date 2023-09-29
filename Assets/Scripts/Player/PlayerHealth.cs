using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Image healthBar; // ������ �� ����������� ������� ��������
    private GameObject deathPanel; // ������ �� ������ ������

    private float currentHealth;
    [SerializeField] private float maxHealth = 100.0f; // ������������ �������� ������

    private JoystickPlayer joystickPlayer; // ������ �� ��������� JoystickPlayer

    private void Start()
    {
        // ����� ��������� JoystickPlayer �� ������ � ��������� ������ �� ����
        joystickPlayer = GetComponent<JoystickPlayer>();

        if (joystickPlayer == null)
        {
            Debug.LogError("�� ������ ��������� JoystickPlayer �� ������.");
        }

        GameObject healthBarObject = GameObject.FindWithTag("HealthBarPlayer");

        if (healthBarObject != null)
        {
            healthBar = healthBarObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("�� ������� ����������� �� � ����� 'healthBarPlayer' �� �����.");
        }

        // ������������� ����� ������ ������ �� �����
        deathPanel = GameObject.Find("Panel Death");

        if (deathPanel == null)
        {
            Debug.LogError("�� ������� ������ ������ �� �����.");
        }
        
        currentHealth = maxHealth;
        UpdateHealthBar();
        deathPanel.SetActive(false); // �� ��������� ������ ������ ���������
    }

    // ������� ��� ��������� �����
    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 0) return; // ���� �������� ��� �������, ����� ��� �����

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(); // ������� ������� ������, ���� �������� �������� ����
        }

        UpdateHealthBar();
    }

    // ������� ��� ���������� ������� ��������
    private void UpdateHealthBar()
    {
        float healthPercent = currentHealth / maxHealth;
        healthBar.fillAmount = healthPercent;
    }

    // ������� ��� ��������� ������
    private void Die()
    {
        // ��������� ����������� ��������� (��������, ��������� ��������� JoystickPlayer)
        if (joystickPlayer != null)
        {
            joystickPlayer.enabled = false;
        }

        // ���������� ������ ������
        deathPanel.SetActive(true);
    }
}
