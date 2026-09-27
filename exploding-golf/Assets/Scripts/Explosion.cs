using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius = 2f;
    public float maxDamage = 100f;

    void Start()
    {
        Explode();
        Destroy(gameObject, 0.1f); // minimalist: explosion is instant
    }

    void Explode()
    {
        // Find all colliders in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            FallingOver piece = hit.GetComponent<FallingOver>();
            if (piece != null)
            {
                // Damage based on distance (closer = more damage)
                float dist = Vector2.Distance(transform.position, piece.transform.position);
                float t = 1f - (dist / radius);
                float damage = Mathf.Clamp(t * maxDamage, 0f, maxDamage);
                piece.ApplyDamage(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}