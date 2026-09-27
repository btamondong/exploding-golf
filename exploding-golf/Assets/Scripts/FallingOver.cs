using UnityEngine;

public class FallingOver : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP = 100f;
    public bool isSupport = false;
    public bool isDead = false;

    [Header("Physics")]
    public Rigidbody2D rb;
    public float fallGravityScale = 2f;
    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHP = maxHP;
    }

    void Update()
    {
        if (!isSupport)
        {
            CheckSupport();
        }
    }

    void CheckSupport()
    {
        if (isSupport) return;

        FallingOver pieceBelow = null;

        // Check straight down
        pieceBelow = RaycastSupport(Vector2.down);

        // Check down-left
        if (pieceBelow == null)
            pieceBelow = RaycastSupport(new Vector2(-0.5f, -1f));

        // Check down-right
        if (pieceBelow == null)
            pieceBelow = RaycastSupport(new Vector2(0.5f, -1f));

        // If still no support → disappear instantly
        if (pieceBelow == null || pieceBelow.isDead)
        {
            StartFalling();
        }
    }

    FallingOver RaycastSupport(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction.normalized, 0.7f);
        if (hit.collider != null)
            return hit.collider.GetComponent<FallingOver>();
        return null;
    }

    public void ApplyDamage(float damage)
    {
        currentHP -= damage;
        if (currentHP <= 0f)
        {
            isDead = true;
            Collider2D col = GetComponent<Collider2D>();
            if (col != null) Destroy(col);
            rb.gravityScale = fallGravityScale;

            // Destroy after physics reacts
            Destroy(gameObject, 0.1f);
            return;
        }
    }
    
    void StartFalling()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic) return;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravityScale;
    }
}
