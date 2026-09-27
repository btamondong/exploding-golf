using System.Collections;
using UnityEngine;
using UnityEngine.UI;
 
public class GolfBallSwing : MonoBehaviour
{
    [Header("Ball Launch Settings")]
    [SerializeField] private float maxLaunchForce = 25f; // max launch speed
    [SerializeField] private float chargeSpeed = 1.5f; // power meter speed
    [SerializeField] private Vector2 launchDirection = new Vector2(0.7f, 0.7f); // x, y directional power

    [Header("Respawn Settings")]
    [SerializeField] private float respawnTimer = 5f; // time it takes for ball to respawn

    [Header("Detonation Settings")]
    public float explosionRadius = 3f; // reach of explosion
    public float maxDamage = 50f; // max damage of explosion to destructible objects
    public GameObject explosionFX;

    public Image ChargeBar;

    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer sprite;
    private float currentPower = 0f;
    private float chargeTime = 0f;
    private bool isCharging = false;
    private bool inMotion = false;
    private Vector2 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sprite = GetComponent<SpriteRenderer>();

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
           ChargeBar.fillAmount = currentPower / 1f;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            FallingOver piece = hit.GetComponent<FallingOver>();
            if (piece != null)
            {
                float dist = Vector2.Distance(transform.position, piece.transform.position);
                float t = 1f - (dist / explosionRadius);
                float damage = Mathf.Clamp(t * maxDamage, 0f, maxDamage);
                piece.ApplyDamage(damage);
            }
        }
        // Hide and disable ball after detonation until respawn
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        col.enabled = false;
        sprite.enabled = false;
    }

    private IEnumerator ResetBallPosition()
    {
        // Wait for a set amount of time before despawning
        yield return new WaitForSeconds(respawnTimer);

        // Set ball back to saved starting position and freeze
        transform.position = startPosition;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        // Reactivate visuals if detonated
        col.enabled = true;
        sprite.enabled = true;

        // Reset current variables for next shot
        currentPower = 0f;
        chargeTime = 0f;
        inMotion = false;
    }

    void OnDrawGizmosSelected()
    {
        // Display explosion radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
