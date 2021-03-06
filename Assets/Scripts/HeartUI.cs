using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUI : MonoBehaviour
{
    [SerializeField] bool isPlayer = false;
    [SerializeField] Character target = null;
    [SerializeField] Heart[] hearts = null;

    List<Heart> currentHearts = new List<Heart>();

    private void Start()
    {
        if(isPlayer)
        {
            target = FindObjectOfType<Player>().GetComponent<Character>();
        }
        initialzieHearts();
    }

    private void Update()
    {
        UpdateHearts();
    }

    void initialzieHearts()
    {
        foreach (Heart heart in hearts)
        {
            heart.gameObject.SetActive(false);
        }

        for (int i = 0; i < target.hp; i++)
        {
            hearts[i].gameObject.SetActive(true);
            currentHearts.Add(hearts[i]);
        }
    }

    void UpdateHearts()
    {
        foreach (Heart heart in currentHearts)
        {
            heart.ShowHeart(0f);
        }

        for (int i = 0; i < target.hp; i++)
        {
            hearts[i].ShowHeart(1f);
            if(target.hp % 1 != 0 && i + 1 > target.hp) hearts[i].ShowHeart(0.5f);    
        }
    }
}
