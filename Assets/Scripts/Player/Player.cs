using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] GameObject _sfxExplosion;

    // Movement
    private float _moveSpeed = 3.5f;
    private float _moveLimitX = 8.5f;
    private float _moveLimitY = 4.5f;

    // Rotation
    private float _rotationSpeed = 1f;
    private float _rotationAngleY = 30f;

    private void Update()
    {
        // ABSTRACTION
        Move();
    }

    /// <summary>
    /// Handle player movement based on input axis.
    /// </summary>
    private void Move()
    {
        float moveDirectionX = 0f, moveDirectionY = 0f, moveDirectionZ = 0f;
        float rotateDirectionX = 0f, rotateDirectionY = 0f, rotateDirectionZ = 0f;

        // Set directions and rotations based on inputs
        moveDirectionX = Input.GetAxis("Horizontal") * Time.deltaTime * _moveSpeed;
        moveDirectionY = Input.GetAxis("Vertical") * Time.deltaTime * _moveSpeed;

        rotateDirectionY = Input.GetAxis("Horizontal") * _rotationAngleY * _rotationSpeed;

        // Move the ship based on directions
        //transform.Translate(moveDirectionX, moveDirectionY, moveDirectionZ);
        transform.position = new Vector3(transform.position.x + moveDirectionX,
                                         transform.position.y + moveDirectionY,
                                         moveDirectionZ);

        // Rotate slightly the ship when moving horizontally
        // Go back to 0 if no horizontal movement
        transform.rotation = Quaternion.Euler(rotateDirectionX, rotateDirectionY, rotateDirectionZ);

        // ONLY WITH TRANSLATE
        // When rotating Z-Axis position is decreasing
        // Reset Z-Axis after ship rotating to prevent it to dissappear
        //transform.position = new Vector3(transform.position.x, transform.position.y, moveDirectionZ);

        // ABSTRACTION
        RepositionIfOutOfGameSpace();
    }

    /// <summary>
    /// Reposition the player to the edges if movement is out of the game space.
    /// </summary>
    private void RepositionIfOutOfGameSpace()
    {
        // Apply X-axis movement constraint
        if (transform.position.x >= _moveLimitX)
            transform.position = new Vector3(_moveLimitX, transform.position.y, transform.position.z);
        if (transform.position.x <= -_moveLimitX)
            transform.position = new Vector3(-_moveLimitX, transform.position.y, transform.position.z);

        // Apply Y-axis movement constraint
        if (transform.position.y >= _moveLimitY)
            transform.position = new Vector3(transform.position.x, _moveLimitY, transform.position.z);
        if (transform.position.y <= -_moveLimitY)
            transform.position = new Vector3(transform.position.x, -_moveLimitY, transform.position.z);
    }

    /// <summary>
    /// Destroy player and asteroids if they collide with each other
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Asteroid") || collision.gameObject.CompareTag("SpaceShip"))
        {
            // Run visual effect
            Instantiate(_sfxExplosion, transform.position, Quaternion.identity);

            // Destroy objects
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// ENCAPSULATION
    /// </summary>
    private float MoveSpeed
    {
        set { _moveSpeed = value; }
        get { return _moveSpeed; }
    }
}
