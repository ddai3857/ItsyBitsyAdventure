using UnityEngine;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Entity : MonoBehaviour
{
    readonly int move_speed = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator Walk(Vector2 next_pos)
    {
        yield return transform.DOMove(new(next_pos.x, next_pos.y, 0), move_speed)
            .SetSpeedBased()
            .SetEase(Ease.Linear).WaitForCompletion();
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
