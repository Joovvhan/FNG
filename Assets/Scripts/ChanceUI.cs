using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChanceUI : MonoBehaviour
{
    [SerializeField] Text chanceText = null;
    [SerializeField] Text chanceCountText = null;
    [SerializeField] ActionIcon[] actionIcons = null;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if (gameManager.isActiveChanceUI)
        {
            chanceText.color = new Color(1f, 1f, 1f, 1f);
            SetActionIconsByChance(true);
        }
        else
        {
            chanceText.color = new Color(1f, 1f, 1f, 0.05f);
            SetActionIconsByChance(false);
        }

        chanceCountText.text = "CHANCE " + gameManager.chanceCount;
    }

    public void SwitchChanceUI(bool on)
    {
        chanceText.gameObject.SetActive(on);
    }

    void SetActionIconsByChance(bool isChance)
    {
        foreach(ActionIcon actionIcon in actionIcons)
        {
            actionIcon.SetImageChance(isChance);
        }
    }
}

[System.Serializable]
public class ActionIcon
{
    [SerializeField] Image image = null;
    [SerializeField] Sprite normal = null;
    [SerializeField] Sprite chance = null;

    public void SetImageChance(bool isChance)
    {
        if(isChance) image.sprite = chance;
        else image.sprite = normal;
    }
}
