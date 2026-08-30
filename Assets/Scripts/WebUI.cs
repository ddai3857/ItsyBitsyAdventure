using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebUI : MonoBehaviour
{

    public List<Sprite> web_states;
    public GameObject webReload;

    public void UpdateWebCount(int count)
    {
        foreach (Transform c in transform)
        {
            c.gameObject.SetActive(count-- > 0);
        }
    }

    public void UpdateWebReload(int count)
    {
        webReload.GetComponent<Image>().sprite = web_states[count];
    }
}
