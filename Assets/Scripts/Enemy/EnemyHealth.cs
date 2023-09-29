using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100.0f; // Максимальное здоровье врага
    private float currentHealth; // Текущее здоровье врага

    private Image healthBarImage; // Ссылка на изображение полоски здоровья врага

    [SerializeField] private GameObject[] deathSpawnObjects; // Массив объектов, которые спавнятся при смерти врага

    private EnemyController enemyController;

    private void Start()
    {
        enemyController = GetComponent<EnemyController>();
        currentHealth = maxHealth; // Устанавливаем начальное здоровье равным максимальному
        healthBarImage = FindClosestHealthBarImage(); // Находим ближайшее изображение полоски здоровья
        UpdateHealthBar(); // Обновляем полоску здоровья при старте
    }

    // Функция для поиска ближайшего изображения полоски здоровья
    private Image FindClosestHealthBarImage()
    {
        Image closestHealthBar = null;
        float closestDistance = float.MaxValue;

        GameObject[] healthBars = GameObject.FindGameObjectsWithTag("HealthBar"); // Предполагается, что полоски здоровья имеют тег "HealthBar"

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

    // Функция для получения урона
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        enemyController.isChasingPlayer = true;
        // Если здоровье меньше или равно нулю, то враг умер
        if (currentHealth <= 0)
        {
            Die();
        }

        UpdateHealthBar(); // Обновляем полоску здоровья после получения урона
    }

    // Функция для обновления полоски здоровья визуально
    private void UpdateHealthBar()
    {
        if (healthBarImage != null)
        {
            float healthPercent = currentHealth / maxHealth;
            healthBarImage.fillAmount = healthPercent;
        }
    }

    // Функция для обработки смерти врага
    private void Die()
    {
        // Получаем позицию и поворот врага
        Vector3 deathPosition = transform.position;
        Quaternion deathRotation = transform.rotation;

        // Спавн случайного объекта из массива при смерти врага
        if (deathSpawnObjects != null && deathSpawnObjects.Length > 0)
        {
            int randomIndex = Random.Range(0, deathSpawnObjects.Length); // Генерируем случайный индекс
            GameObject randomSpawnObject = deathSpawnObjects[randomIndex]; // Получаем случайный объект
            Instantiate(randomSpawnObject, deathPosition, deathRotation);
        }

        // например, уничтожение объекта врага или воспроизведение анимации смерти.
        Destroy(transform.parent.gameObject); // Простой пример: уничтожение объекта врага.
    }
}
