using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    public float explosionRadius = 8f;
    public float maxDamage = 300f;
    public GameObject explosionFX;

    void OnCollisionEnter2D(Collision2D collision)
    {
        float impact = collision.relativeVelocity.magnitude;

        // Only explode if the impact is strong enough
        if (impact > 4f)
        {
            Explode();
        }
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
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}