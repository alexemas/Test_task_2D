using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer; // Слой с врагами
    [SerializeField] private Transform firePoint; // Точка, откуда будут выпускаться пули
    [SerializeField] private GameObject bulletPrefab; // Префаб пули
    [SerializeField] private float shootingRange = 5f; // Радиус для стрельбы
    [SerializeField] private float bulletSpeed;
    private TMP_Text ammoText; // Ссылка на текст UI для отображения патронов

    [SerializeField] private int maxAmmo = 10; // Максимальное количество патронов
    private int currentAmmo; // Текущее количество патронов

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentAmmo = maxAmmo;

        // Находим объект с компонентом TMP_Text и именем "Bullet Text"
        ammoText = FindObjectOfType<TMP_Text>();

        if (ammoText == null)
        {
            Debug.LogError("Не найден объект с именем 'Bullet Text' и компонентом TMP_Text на сцене.");
        }

        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo + "/" + maxAmmo;
        }
    }

    private bool IsEnemyInRange()
    {
        // Используем Physics2D.OverlapCircle для проверки наличия врага в радиусе
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, shootingRange, enemyLayer);

        // Проверяем все коллайдеры в радиусе
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }

    public void Shoot()
    {
        // Проверяем наличие патронов
        if (currentAmmo > 0 && IsEnemyInRange())
        {
            currentAmmo--;
            UpdateAmmoUI();

            // Находим ближайшего врага
            Collider2D nearestEnemyCollider = FindNearestEnemy();
            if (nearestEnemyCollider != null)
            {
                EnemyController nearestEnemyController = nearestEnemyCollider.GetComponent<EnemyController>();

                nearestEnemyController.isChasingPlayer = true;

                // Определяем направление к ближайшему врагу
                Vector2 direction = (nearestEnemyCollider.transform.position - transform.position).normalized;

                // Если направление игрока (взгляда) смотрит в сторону врага (direction.x > 0), то не выполнять флип
                if (direction.x < 0 && playerController.IsFacingRight())
                {
                    playerController.Flip();
                }
                else if (direction.x > 0 && !playerController.IsFacingRight())
                {
                    playerController.Flip();
                }

                // Создаем пулю
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                // Устанавливаем направление движения пули в сторону ближайшего врага
                bullet.transform.right = direction; // Поворачиваем пулю в сторону врага

                // Устанавливаем скорость движения пули
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = direction * bulletSpeed;
            }
        }
        else
        {
            ReloadAmmo();
        }
    }


    public void ReloadAmmo()
    {
        if (currentAmmo < 30)
        {
            // Поиск ячеек в инвентаре с патронами "5.45x39"
            InventoryCell[] ammoCells = Inventory.Instance.GetAmmoCells("5.45x39");

            if (ammoCells.Length > 0)
            {
                // Находим индекс последней ячейки с патронами "5.45x39"
                int lastIndex = ammoCells.Length - 1;

                // Получаем количество патронов в последней ячейке
                int lastStackCount = ammoCells[lastIndex].StackCount;

                // Удаление 1 патрона из последней ячейки или удаление ячейки, если в ней 1 патрон
                if (lastStackCount > 1)
                {
                    ammoCells[lastIndex].RemoveItem(1);
                }
                else
                {
                    Inventory.Instance.RemoveItem(ammoCells[lastIndex]);
                }

                // Добавляем максимальное количество патронов к текущему боезапасу
                currentAmmo += maxAmmo-currentAmmo;
                UpdateAmmoUI();

                Debug.Log("Перезарядка: +" + maxAmmo + " патронов");
            }
            else
            {
                Debug.Log("Нет патронов в инвентаре.");
            }
        }

    }

    private Collider2D FindNearestEnemy()
    {
        // Находим ближайшего врага
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, shootingRange, enemyLayer);
        Collider2D nearestEnemyCollider = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D collider in hitColliders)
        {
            float distance = Vector2.Distance(transform.position, collider.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemyCollider = collider;
            }
        }

        return nearestEnemyCollider;
    }
}
