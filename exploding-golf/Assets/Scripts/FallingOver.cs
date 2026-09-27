using UnityEngine;

public class FallingOver : MonoBehaviour
{
    [Header("Stats")]
    public float maxHP = 100f;
    public float currentHP = 100f;
    public bool isSupport = false;

    [Header("Physics")]
    public Rigidbody2D rb;

    private SpriteRenderer sr;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.bodyType = RigidbodyType2D.Kinematic;
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
        if (pieceBelow == null || pieceBelow.currentHP <= 0f)
        {
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(gameObject);
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

        if (sr != null)
        {
            sr.color = Color.white;
            CancelInvoke(nameof(ResetColor));
            Invoke(nameof(ResetColor), 0.05f);
        }

        if (currentHP <= 0f)
        {
            Destroy(GetComponent<Collider2D>());
            Destroy(GetComponent<SpriteRenderer>());
            Destroy(gameObject);
        }
        Debug.Log("Damage applied to: " + gameObject.name + " HP now: " + currentHP);
        Debug.Log("Damage: " + damage);

    }

    void ResetColor()
    {
        if (sr != null)
            sr.color = Color.gray;
    }
}
