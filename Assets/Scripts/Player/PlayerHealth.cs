using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private Image healthBar; // Ссылка на изображение полоски здоровья
    private GameObject deathPanel; // Ссылка на панель смерти

    private float currentHealth;
    [SerializeField] private float maxHealth = 100.0f; // Максимальное здоровье игрока

    private JoystickPlayer joystickPlayer; // Ссылка на компонент JoystickPlayer

    private void Start()
    {
        // Найти компонент JoystickPlayer на игроке и сохранить ссылку на него
        joystickPlayer = GetComponent<JoystickPlayer>();

        if (joystickPlayer == null)
        {
            Debug.LogError("Не найден компонент JoystickPlayer на игроке.");
        }

        GameObject healthBarObject = GameObject.FindWithTag("HealthBarPlayer");

        if (healthBarObject != null)
        {
            healthBar = healthBarObject.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("Не найдено изображение хп с тегом 'healthBarPlayer' на сцене.");
        }

        // Автоматически найти панель смерти на сцене
        deathPanel = GameObject.Find("Panel Death");

        if (deathPanel == null)
        {
            Debug.LogError("Не найдена панель смерти на сцене.");
        }
        
        currentHealth = maxHealth;
        UpdateHealthBar();
        deathPanel.SetActive(false); // По умолчанию панель смерти отключена
    }

    // Функция для получения урона
    public void TakeDamage(float damageAmount)
    {
        if (currentHealth <= 0) return; // Если здоровье уже нулевое, игрок уже мертв

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(); // Вызвать функцию смерти, если здоровье достигло нуля
        }

        UpdateHealthBar();
    }

    // Функция для обновления полоски здоровья
    private void UpdateHealthBar()
    {
        float healthPercent = currentHealth / maxHealth;
        healthBar.fillAmount = healthPercent;
    }

    // Функция для обработки смерти
    private void Die()
    {
        // Отключить возможность двигаться (например, отключить компонент JoystickPlayer)
        if (joystickPlayer != null)
        {
            joystickPlayer.enabled = false;
        }

        // Отобразить панель смерти
        deathPanel.SetActive(true);
    }
}
