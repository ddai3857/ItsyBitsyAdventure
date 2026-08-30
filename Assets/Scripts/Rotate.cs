using System.Collections;
using DG.Tweening;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;

public class Rotate : MonoBehaviour
{

    float rot_amount = 5;
    float button_scale = 1.2F;
    float rot_time = 4;
    public bool flip = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (flip)
        {
            rot_amount *= -1;
        }
        StartCoroutine(Rot());
    }

    public IEnumerator Rot()
    {
        while (true)
        {
            yield return transform.DORotate(new (0,0,rot_amount), rot_time)
                .SetEase(Ease.InOutSine)
                .WaitForCompletion();
            yield return transform.DORotate(new (0,0,-rot_amount), rot_time)
                .SetEase(Ease.InOutSine)
                .WaitForCompletion();   
        }
    }

    public void StartButton()
    {
        print("Start");
    }    
    
    public void MenuButton()
    {
        print("Menu");
    }

    public void QuitButton()
    {
        Application.Quit();
        print("Quit");
    }

    public void OnPointerEnter()
    {
        transform.DOScale(button_scale, 1)
            .SetEase(Ease.OutCubic);
    }

    // Triggered automatically when mouse leaves the button area
    public void OnPointerExit()
    {
        transform.DOScale(1, 1)
            .SetEase(Ease.OutCubic);
    }
}
