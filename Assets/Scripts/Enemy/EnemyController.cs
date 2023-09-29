using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f; // �������� �������� �����
    [SerializeField] private float detectionRadius = 5f; // ������ ����������� ������
    [SerializeField] private float stopDistace = 1f;
    [SerializeField] private LayerMask obstacleLayer; // ����, �� ������� ��������� �����������
    private Transform player; // ������ �� ������
    private bool isFacingRight = true; // ����, ����������� ����������� ������� �����

    public bool isChasingPlayer = false; // ����, �����������, ��� ���� ���������� ������

    private Rigidbody2D rb; // ��������� Rigidbody2D �����

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>(); // �������� ��������� Rigidbody2D � ������
    }

    private void Update()
    {
        // ��������� ���������� �� ������
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ���� ����� ��������� � ������� ����������� � ���� �� ���������� ���
        if (distanceToPlayer <= detectionRadius && !isChasingPlayer)
        {
            // ���� �������� �������������
            isChasingPlayer = true;
        }

        // ���� ���� ���������� ������ ��� ����� ��� ��� ��������� � ������� �����������
        if (isChasingPlayer)
        {
            // ������������ ����� � ������� ������
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // ���������, �� ��������� �� ���� ����� ������ ����� ������������
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Abs(direction.x) + 0.3f, obstacleLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Abs(direction.x) + 0.3f, obstacleLayer);
            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Abs(direction.y) + 0.75f, obstacleLayer);
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Abs(direction.y) + 0.75f, obstacleLayer);

            // ����������� �����, ���� ��� ������������ � ���������������� �������
            if (!(direction.x > 0 && hitRight.collider != null) &&
                !(direction.x < 0 && hitLeft.collider != null) &&
                !(direction.y > 0 && hitUp.collider != null) &&
                !(direction.y < 0 && hitDown.collider != null))
            {
                if (distanceToPlayer <= stopDistace)
                {

                }
                else
                {
                    // ���� �� �������� ������, ������� ���������� ��������
                    transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                }
            }

            // ���������, ����� �� ���������� �����
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
            {
                Flip();
            }
        }
        if (distanceToPlayer >= detectionRadius * 2)
            isChasingPlayer = false; // ���� ��������� ������ ��������� ��� ���� ������ �����������, ���������� �������������
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
