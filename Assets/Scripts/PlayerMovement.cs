using UnityEngine;

public class PlayerMovement : MonoBehaviour, ISaveable {

    private float x, z;
    private Vector3 velocity;
    private bool isGrounded;

    public CharacterController controller;
    public float speed = 2.5f;
    public float gravity = -9.81f;
    public float gravityMultiplier = 4f;
    public Transform groundCheck;
    public float groundDistance;
    public float jumpHeight = 3f;
    public LayerMask groundMask;

    void Update() {
        if (PauseMenu.IsPaused) return;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) velocity.y = -2f;

        x = Input.GetAxis("Horizontal");    // GetAxisRaw() for snappy movement, GetAxis() for lerp.
        z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        Physics.SyncTransforms();           // Required for controller.Move() not to intermittently reset player position after being set in Load(), see: https://stackoverflow.com/questions/59270616/when-i-try-to-transform-this-player-controller-it-just-resets-to-the-original-po
        controller.Move(speed * Time.deltaTime * move);

        if (Input.GetButtonDown("Jump") && isGrounded) 
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * gravityMultiplier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Save(SaveData data)
    {
        data.PlayerPosition = controller.transform.position;
    }

    public void Load(SaveData data)
    {
        controller.transform.position = data.PlayerPosition;
    }
}
