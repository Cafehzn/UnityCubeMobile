using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 movementInput;
    public ParticleSystem playerDestruction;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxSpeed = 20f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        Vector3 moveDirection = 
            new Vector3(movementInput.x, 0f, movementInput.y) * speed;

        if(rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.linearVelocity =
            new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
        }
        
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Instantiate(playerDestruction, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
