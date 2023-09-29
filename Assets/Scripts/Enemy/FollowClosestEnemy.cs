using UnityEngine;

public class FollowClosestEnemy : MonoBehaviour
{
    private Transform target; // Трансформ цели (ближайшего врага)
    private Vector3 offset; // Смещение между канвасом и целью

    private void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            // Находим ближайшего врага среди всех объектов с заданным тегом
            float closestDistance = Mathf.Infinity;

            foreach (GameObject enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = enemy.transform;
                    offset = transform.position - target.position;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            // Обновляем позицию канваса сразу к цели
            transform.position = target.position + offset;
        }
        else
        {
            // Если цель отсутствует (например, враги были уничтожены), перестаем следовать
            target = null;
        }
    }
}
