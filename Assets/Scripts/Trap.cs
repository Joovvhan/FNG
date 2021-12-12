using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] GameObject needle;
    [SerializeField] private int needleCycle;
    protected GameManager gameManager;
    protected Player player;
    private int turn = 0;


    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddTrapToList(this);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public IEnumerator SetNeedle()
    {
        turn += 1;
        if (turn % needleCycle == 0)
        {
            needle.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            if (gameManager.GetPlayerPosition() == (int)transform.position.x)
            {
                yield return StartCoroutine(player.LoseHP(1.0f));
            }
            yield return new WaitForSeconds(0.6f);
            needle.SetActive(false);
        }
        else
        {
            needle.SetActive(false);
        }
    }

}
