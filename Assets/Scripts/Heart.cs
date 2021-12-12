using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class Heart : MonoBehaviour
{
    [SerializeField] ProceduralImage fill = null;
    [SerializeField] Image background = null;

    public void ShowHeart(float amount)
    {
        fill.fillAmount = amount;
    }
}
