using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Entity : MonoBehaviour
{
    public int speed;
    [SerializeField]
    int move_speed;
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
            .SetEase(Ease.Linear);
    }
}
