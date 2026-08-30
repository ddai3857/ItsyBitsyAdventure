using System.Collections;
using DG.Tweening;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Called Rotate but holds a lot of extra code for menus and stuff too
public class Rotate : MonoBehaviour
{
    float rot_amount = 5;
    float button_scale = 1.2F;
    float rot_time = 4;
    public bool flip = false;
    public GameObject menu_screen; // throwing evrything here. Do not copy this terrible code :skull:
    public static bool menu_on = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (flip)
        {
            rot_amount *= -1;
        }
        StartCoroutine(Rot());
        menu_on = false;
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
        if (!menu_on)
        {
            print("Start");
        }
    }    
    
    public void MenuButton()
    {
        if (!menu_on)
        {
            print("Menu");
            menu_screen.SetActive(true);
            menu_screen.GetComponent<RectTransform>().DOAnchorPosY(0, 2)
                .SetEase(Ease.OutElastic, 0.1F, 1)
                .WaitForCompletion(); 
            menu_on = true;
        }
    }
    public void MenuBackButton()
    {
        if (menu_on)
        {
            print("MenuBack");
            menu_screen.transform.DOMoveY(2000, 1)
            .SetEase(Ease.InElastic, 0.1F, 1)
            .OnComplete(() => menu_screen.SetActive(false));
            menu_on = false;
        }
    }
    public void QuitButton()
    {
        if (!menu_on)
        {
            print("Quit");
            Application.Quit();
        }
    }

    public void OnPointerEnter()
    {
        if (!menu_on || tag == "Menu")
        {
            transform.DOScale(button_scale, 1)
                .SetEase(Ease.OutCubic);
        }
    }

    // Triggered automatically when mouse leaves the button area
    public void OnPointerExit()
    {
        if (!menu_on || tag == "Menu")
        {
            transform.DOScale(1, 1)
                .SetEase(Ease.OutCubic);
        }
    }
}
