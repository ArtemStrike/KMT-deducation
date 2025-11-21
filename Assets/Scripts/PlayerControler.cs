using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 6.0f;
    public float acceleration = 10.0f;

    [Header("Jump Settings")]
    public float jumpForce = 5.0f;
    public float groundCheckDistance = 0.1f;
    public LayerMask groundLayer;

    public Rigidbody Player;
    private bool isGrounded = false;

    void Start()
    {
        if (Player == null)
        {
            Player = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        // Проверка земли через Raycast (более надежный способ)
        isGrounded = Physics.Raycast(transform.position, Vector3.down,
                                     GetComponent<Collider>().bounds.extents.y + groundCheckDistance);

        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey("w")) moveDirection += transform.forward;
        if (Input.GetKey("s")) moveDirection -= transform.forward;
        if (Input.GetKey("a")) moveDirection -= transform.right;
        if (Input.GetKey("d")) moveDirection += transform.right;

        if (moveDirection != Vector3.zero)
        {
            Vector3 targetVelocity = moveDirection.normalized * speed;
            Vector3 currentVelocity = new Vector3(Player.linearVelocity.x, 0, Player.linearVelocity.z);
            Vector3 newVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
            Player.linearVelocity = new Vector3(newVelocity.x, Player.linearVelocity.y, newVelocity.z);
        }
        else
        {
            // Плавная остановка
            Vector3 currentVelocity = new Vector3(Player.linearVelocity.x, 0, Player.linearVelocity.z);
            Vector3 newVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, acceleration * Time.fixedDeltaTime);
            Player.linearVelocity = new Vector3(newVelocity.x, Player.linearVelocity.y, newVelocity.z);
        }
    }

    void Update()
    {
        // Прыжок
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Player.linearVelocity = new Vector3(Player.linearVelocity.x, 0, Player.linearVelocity.z);
            Player.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Разблокировка курсора
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
