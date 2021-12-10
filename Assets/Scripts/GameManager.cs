using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //public static GameManager instance = null;
    private BoardManager boardScript;
    public float turnDelay = 0.1f;
    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public bool enemiesMoving = false;
    private List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        //if (instance == null)
        //    instance = this;
        //else if (instance != this)
        //    Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        boardScript = GetComponent<BoardManager>();
        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene();
    }


    public void GameOver()
    {
        Debug.Log("Game Over");
        enabled = false;
    }

    public void AddEnemyToList(Enemy script)
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);

        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }
        playersTurn = true;
        enemiesMoving = false;
    }

    void Update()
    {
        if (playersTurn || enemiesMoving)
            return;
        StartCoroutine(MoveEnemies());
    }


}
