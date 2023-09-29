using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Скорость движения персонажа
    private GameObject player; // Ссылка на объект персонажа
    [SerializeField] private LayerMask wallLayer; // Слой "Wall" для определения стен
    private bool facingRight = true; // Флаг, указывающий направление взгляда персонажа

    private void Start()
    {
        player = gameObject; // Получаем ссылку на объект персонажа
    }

    public void Move(Vector2 movement)
    {
        var horizontal = movement.x * moveSpeed * Time.deltaTime; // Вычисляем горизонтальное движение
        var vertical = movement.y * moveSpeed * Time.deltaTime; // Вычисляем вертикальное движение

        Vector2 newPosition = (Vector2)transform.position + new Vector2(horizontal, vertical); // Вычисляем новую позицию персонажа

        // Проверяем столкновение с помощью лучей в разных направлениях
        RaycastHit2D hitRight = Physics2D.Raycast(newPosition, Vector2.right, Mathf.Abs(horizontal) + 0.3f, wallLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(newPosition, Vector2.left, Mathf.Abs(horizontal) + 0.3f, wallLayer);
        RaycastHit2D hitUp = Physics2D.Raycast(newPosition, Vector2.up, Mathf.Abs(vertical) + 0.75f, wallLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(newPosition, Vector2.down, Mathf.Abs(vertical) + 0.75f, wallLayer);

        // Перемещаем персонаж, если нет столкновений с соответствующими стенами
        if (!(horizontal > 0 && hitRight.collider != null) &&
            !(horizontal < 0 && hitLeft.collider != null))
        {
            player.transform.Translate(new Vector2(horizontal, 0f));
        }

        if (!(vertical > 0 && hitUp.collider != null) &&
            !(vertical < 0 && hitDown.collider != null))
        {
            player.transform.Translate(new Vector2(0f, vertical));
        }

        // Определяем направление движения и разворачиваем персонажа
        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }
    }

    public void Flip()
    {
        facingRight = !facingRight; // Инвертируем флаг направления взгляда

        Vector3 scale = player.transform.localScale; // Получаем масштаб персонажа

        scale.x *= -1; // Инвертируем масштаб по оси X для разворота

        player.transform.localScale = scale; // Применяем измененный масштаб
    }

    public bool IsFacingRight()
    {
        return facingRight; // Возвращает true, если персонаж смотрит вправо
    }
}
