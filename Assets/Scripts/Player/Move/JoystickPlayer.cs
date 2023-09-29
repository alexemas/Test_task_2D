using UnityEngine;

public class JoystickPlayer : MonoBehaviour
{
    private FixedJoystick fixedJoystick;
    [SerializeField] private Rigidbody2D rb;

    private PlayerController playerController;

    private void Start()
    {
        // Ищем объект FixedJoystick на сцене и присваиваем его переменной fixedJoystick
        fixedJoystick = FindObjectOfType<FixedJoystick>();
        playerController = GetComponent<PlayerController>();
    }

    private void FixedUpdate()
    {
        if (fixedJoystick != null)
        {
            Vector2 movement = (Vector2.up * fixedJoystick.Vertical + Vector2.right * fixedJoystick.Horizontal).normalized;
            playerController.Move(movement);
        }
    }
}
