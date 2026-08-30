using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    readonly int move_speed = 2;
    bool sprite_fliped = true;
    bool moving = false;
    float idle_flip_speed = 1f;
    float walking_flip_speed = 1f;
    
    public static Ease moving_ease;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpriteFlipLoop());
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public IEnumerator SpriteFlipLoop()
    {
        float flip_speed = idle_flip_speed;
        if (moving)
        {
            flip_speed = walking_flip_speed;
        }
        GetComponent<SpriteRenderer>().flipX = sprite_fliped;
        yield return new WaitForSeconds(flip_speed);
        sprite_fliped = !sprite_fliped;
        StartCoroutine(SpriteFlipLoop());
    }

    public IEnumerator Walk(Vector2 next_pos)
    {
        moving = true;
        yield return transform.DOMove(new(next_pos.x, next_pos.y, 0), move_speed)
            .SetSpeedBased()
            .SetEase(moving_ease).WaitForCompletion();
        moving = false;
    }

    public virtual IEnumerator Squish()
    {
        yield return transform.DOScaleY(0.5f, 0.2f).SetEase(Ease.Linear).WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public virtual IEnumerator Shrink()
    {
        yield return transform.DOScale(0, 0.5f).SetEase(Ease.Linear).WaitForCompletion();
        Destroy(gameObject);
    }
}
