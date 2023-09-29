using TMPro;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer; // ���� � �������
    [SerializeField] private Transform firePoint; // �����, ������ ����� ����������� ����
    [SerializeField] private GameObject bulletPrefab; // ������ ����
    [SerializeField] private float shootingRange = 5f; // ������ ��� ��������
    [SerializeField] private float bulletSpeed;
    private TMP_Text ammoText; // ������ �� ����� UI ��� ����������� ��������

    [SerializeField] private int maxAmmo = 10; // ������������ ���������� ��������
    private int currentAmmo; // ������� ���������� ��������

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        currentAmmo = maxAmmo;

        // ������� ������ � ����������� TMP_Text � ������ "Bullet Text"
        ammoText = FindObjectOfType<TMP_Text>();

        if (ammoText == null)
        {
            Debug.LogError("�� ������ ������ � ������ 'Bullet Text' � ����������� TMP_Text �� �����.");
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
        // ���������� Physics2D.OverlapCircle ��� �������� ������� ����� � �������
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, shootingRange, enemyLayer);

        // ��������� ��� ���������� � �������
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
        // ��������� ������� ��������
        if (currentAmmo > 0 && IsEnemyInRange())
        {
            currentAmmo--;
            UpdateAmmoUI();

            // ������� ���������� �����
            Collider2D nearestEnemyCollider = FindNearestEnemy();
            if (nearestEnemyCollider != null)
            {
                EnemyController nearestEnemyController = nearestEnemyCollider.GetComponent<EnemyController>();

                nearestEnemyController.isChasingPlayer = true;

                // ���������� ����������� � ���������� �����
                Vector2 direction = (nearestEnemyCollider.transform.position - transform.position).normalized;

                // ���� ����������� ������ (�������) ������� � ������� ����� (direction.x > 0), �� �� ��������� ����
                if (direction.x < 0 && playerController.IsFacingRight())
                {
                    playerController.Flip();
                }
                else if (direction.x > 0 && !playerController.IsFacingRight())
                {
                    playerController.Flip();
                }

                // ������� ����
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

                // ������������� ����������� �������� ���� � ������� ���������� �����
                bullet.transform.right = direction; // ������������ ���� � ������� �����

                // ������������� �������� �������� ����
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
            // ����� ����� � ��������� � ��������� "5.45x39"
            InventoryCell[] ammoCells = Inventory.Instance.GetAmmoCells("5.45x39");

            if (ammoCells.Length > 0)
            {
                // ������� ������ ��������� ������ � ��������� "5.45x39"
                int lastIndex = ammoCells.Length - 1;

                // �������� ���������� �������� � ��������� ������
                int lastStackCount = ammoCells[lastIndex].StackCount;

                // �������� 1 ������� �� ��������� ������ ��� �������� ������, ���� � ��� 1 ������
                if (lastStackCount > 1)
                {
                    ammoCells[lastIndex].RemoveItem(1);
                }
                else
                {
                    Inventory.Instance.RemoveItem(ammoCells[lastIndex]);
                }

                // ��������� ������������ ���������� �������� � �������� ���������
                currentAmmo += maxAmmo-currentAmmo;
                UpdateAmmoUI();

                Debug.Log("�����������: +" + maxAmmo + " ��������");
            }
            else
            {
                Debug.Log("��� �������� � ���������.");
            }
        }

    }

    private Collider2D FindNearestEnemy()
    {
        // ������� ���������� �����
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
