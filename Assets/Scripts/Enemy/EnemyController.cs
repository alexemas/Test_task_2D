using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f; // Скорость движения врага
    [SerializeField] private float detectionRadius = 5f; // Радиус обнаружения игрока
    [SerializeField] private float stopDistace = 1f;
    [SerializeField] private LayerMask obstacleLayer; // Слой, на котором находятся препятствия
    private Transform player; // Ссылка на игрока
    private bool isFacingRight = true; // Флаг, указывающий направление взгляда врага

    public bool isChasingPlayer = false; // Флаг, указывающий, что враг преследует игрока

    private Rigidbody2D rb; // Компонент Rigidbody2D врага

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>(); // Получаем компонент Rigidbody2D в начале
    }

    private void Update()
    {
        // Вычисляем расстояние до игрока
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Если игрок находится в радиусе обнаружения и враг не преследует его
        if (distanceToPlayer <= detectionRadius && !isChasingPlayer)
        {
            // Враг начинает преследование
            isChasingPlayer = true;
        }

        // Если враг преследует игрока или игрок все еще находится в радиусе обнаружения
        if (isChasingPlayer)
        {
            // Поворачиваем врага в сторону игрока
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Проверяем, не находится ли враг перед стеной перед перемещением
            RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Abs(direction.x) + 0.3f, obstacleLayer);
            RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Abs(direction.x) + 0.3f, obstacleLayer);
            RaycastHit2D hitUp = Physics2D.Raycast(transform.position, Vector2.up, Mathf.Abs(direction.y) + 0.75f, obstacleLayer);
            RaycastHit2D hitDown = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Abs(direction.y) + 0.75f, obstacleLayer);

            // Передвигаем врага, если нет столкновений с соответствующими стенами
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
                    // Враг не касается игрока, поэтому продолжаем движение
                    transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                }
            }

            // Проверяем, нужно ли развернуть врага
            if ((direction.x > 0 && !isFacingRight) || (direction.x < 0 && isFacingRight))
            {
                Flip();
            }
        }
        if (distanceToPlayer >= detectionRadius * 2)
            isChasingPlayer = false; // Если дистанция игрока превысила два раза радиус обнаружения, прекращаем преследование
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
