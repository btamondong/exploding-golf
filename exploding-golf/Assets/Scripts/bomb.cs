using UnityEngine;

public class Bomb : MonoBehaviour
{
    [Header("Bomb Settings")]
    public float explosionRadius = 8f;
    public float maxDamage = 300f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        Debug.Log("Bomb exploded, hits: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            FallingOver piece = hit.GetComponent<FallingOver>();
            if (piece != null)
            {
                float dist = Vector2.Distance(transform.position, piece.transform.position);
                float t = 1f - (dist / explosionRadius);
                float damage = Mathf.Clamp(t * maxDamage, 0f, maxDamage);
                Debug.Log("Damage applied to: " + piece.name + " amount: " + damage);
                piece.ApplyDamage(damage);
                Debug.Log("Distance to " + piece.name + ": " + dist);
            }
        }

        Destroy(gameObject);
        // Optional minimalist flash
        // CameraShake.Shake();  <-- if you add one later

        Destroy(gameObject);
        Debug.Log("Bomb exploded, hits: " + hits.Length);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}