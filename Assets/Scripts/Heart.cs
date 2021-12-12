using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    [SerializeField] Image fill = null;
    [SerializeField] Image background = null;

    public void ShowHeart(bool willShow)
    {
        fill.gameObject.SetActive(willShow);
    }
}
