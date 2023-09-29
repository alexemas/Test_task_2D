using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // �������� �������� ���������
    private GameObject player; // ������ �� ������ ���������
    [SerializeField] private LayerMask wallLayer; // ���� "Wall" ��� ����������� ����
    private bool facingRight = true; // ����, ����������� ����������� ������� ���������

    private void Start()
    {
        player = gameObject; // �������� ������ �� ������ ���������
    }

    public void Move(Vector2 movement)
    {
        var horizontal = movement.x * moveSpeed * Time.deltaTime; // ��������� �������������� ��������
        var vertical = movement.y * moveSpeed * Time.deltaTime; // ��������� ������������ ��������

        Vector2 newPosition = (Vector2)transform.position + new Vector2(horizontal, vertical); // ��������� ����� ������� ���������

        // ��������� ������������ � ������� ����� � ������ ������������
        RaycastHit2D hitRight = Physics2D.Raycast(newPosition, Vector2.right, Mathf.Abs(horizontal) + 0.3f, wallLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(newPosition, Vector2.left, Mathf.Abs(horizontal) + 0.3f, wallLayer);
        RaycastHit2D hitUp = Physics2D.Raycast(newPosition, Vector2.up, Mathf.Abs(vertical) + 0.75f, wallLayer);
        RaycastHit2D hitDown = Physics2D.Raycast(newPosition, Vector2.down, Mathf.Abs(vertical) + 0.75f, wallLayer);

        // ���������� ��������, ���� ��� ������������ � ���������������� �������
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

        // ���������� ����������� �������� � ������������� ���������
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
        facingRight = !facingRight; // ����������� ���� ����������� �������

        Vector3 scale = player.transform.localScale; // �������� ������� ���������

        scale.x *= -1; // ����������� ������� �� ��� X ��� ���������

        player.transform.localScale = scale; // ��������� ���������� �������
    }

    public bool IsFacingRight()
    {
        return facingRight; // ���������� true, ���� �������� ������� ������
    }
}
