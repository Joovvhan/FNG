using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    //public static GameManager instance = null;
    private BoardManager boardScript;
    private float turnDelay = 0.5f;
    private float unitDelay = 0.2f;

    private float eps_t = 0.01f;

    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public bool playerMoving = false;
    [HideInInspector] public bool enemiesMoving = false;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    private UnityEngine.UI.Text text;
    private Player player;
    private int chanceTurn = 0;

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        text = GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>();

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

    public void SetPlayer(Player script)
    {
        player = script;
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
            yield return StartCoroutine(enemies[i].MoveEnemy());
            //Debug.Log("Enemy move");
            //yield return new WaitForSeconds(enemies[i].moveTime);
            //Debug.Log("Next Enemy move");

        }
        playersTurn = true;
        enemiesMoving = false;
        StepTurn();
    }

    void Update()
    {
        if (playersTurn || playerMoving || enemiesMoving)
        {
            SetText();
            return;
        }
        StartCoroutine(MoveEnemies());
    }

    void SetText()
    {
        if (playerMoving)
        {
            text.text = "Player Moving" + "/" + player.hp.ToString() + "/" + chanceTurn.ToString();
        }
        else if (playersTurn)
        {
            text.text = "Player's Turn" + "/" + player.hp.ToString() + "/" + chanceTurn.ToString();
        }
        else if (enemiesMoving)
        {
            text.text = "Enemy Moving" + "/" + player.hp.ToString() + "/" + chanceTurn.ToString();
        }
    }

    public int GetChanceTurn()
    {
        return chanceTurn;
    }

    public void StepTurn()
    {
        chanceTurn = (chanceTurn + 1) % 3;
    }

}
