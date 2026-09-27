using System.Collections;
using UnityEngine;

public class GolfBallSwing : MonoBehaviour
{
    [Header("Ball Launch Settings")]
    [SerializeField] private float maxLaunchForce = 25f; // max launch speed
    [SerializeField] private float chargeSpeed = 1.5f; // power meter speed
    [SerializeField] private Vector2 launchDirection = new Vector2(0.7f, 0.7f); // x, y directional power

    [Header("Respawn Settings")]
    [SerializeField] private float respawnTimer = 5f; // time it takes for ball to respawn

    private Rigidbody2D rb;
    private float currentPower = 0f;
    private float chargeTime = 0f;
    private bool isCharging = false;
    private bool inMotion = false;
    private Vector2 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Set starting position in scene to a variable
        startPosition = transform.position;

        // Freeze ball in place until it is launched
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Handle player mouse click inputs
        if (Input.GetMouseButtonDown(0) && !inMotion)
        {
            if (isCharging == false)
            {
                // On first click, start charging
                isCharging = true;
            }
            else if (isCharging == true)
            {
                // On second click, lock power and launch ball
                LaunchBall();
            }
        }

        // Run meter cycle when in charging state
        if (isCharging == true)
        {
            // Loop continuously while in charging state, with power going between 0-1
           chargeTime += Time.deltaTime * chargeSpeed;
           currentPower = Mathf.PingPong(chargeTime, 1f);
        }
    }

    // Function to launch ball based on set power after second click
    private void LaunchBall()
    {
        // Stop charging state once ball is released
        isCharging = false;

        // Prevent player from launching ball again while in motion
        inMotion = true;
        
        // Reapply gravity to ball after launch
        rb.bodyType = RigidbodyType2D.Dynamic;

        // Multiply max launch value by 0-1 multiplier
        float finalForce = currentPower * maxLaunchForce;

        // Apply force to the ball based on direction and final force
        rb.AddForce(launchDirection.normalized * finalForce, ForceMode2D.Impulse);

        // Start respawn function
        StartCoroutine(ResetBallPosition());
    }

    private IEnumerator ResetBallPosition()
    {
        // Wait for a set amount of time before despawning
        yield return new WaitForSeconds(respawnTimer);

        // Set ball back to saved starting position and freeze
        transform.position = startPosition;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        // Reset current variables for next shot
        currentPower = 0f;
        chargeTime = 0f;
        inMotion = false;
    }
}
