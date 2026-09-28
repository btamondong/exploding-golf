using UnityEngine;

public class GolferAnimator : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite chargingSprite;
    public Sprite swungSprite;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite;
    }

    public void SetIdle()
    {
        sr.sprite = idleSprite;
    }

    public void SetCharging()
    {
        sr.sprite = chargingSprite;
    }

    public void SetSwung()
    {
        sr.sprite = swungSprite;
    }
}