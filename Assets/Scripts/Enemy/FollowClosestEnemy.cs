using UnityEngine;

public class FollowClosestEnemy : MonoBehaviour
{
    private Transform target; // ��������� ���� (���������� �����)
    private Vector3 offset; // �������� ����� �������� � �����

    private void Start()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length > 0)
        {
            // ������� ���������� ����� ����� ���� �������� � �������� �����
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
            // ��������� ������� ������� ����� � ����
            transform.position = target.position + offset;
        }
        else
        {
            // ���� ���� ����������� (��������, ����� ���� ����������), ��������� ���������
            target = null;
        }
    }
}
