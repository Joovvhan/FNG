using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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

        int disabledCount = 0;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
            {
                yield return StartCoroutine(enemies[i].MoveEnemy());
            }
            else
            {
                disabledCount += 1;
            }
        }

        if (disabledCount == enemies.Count)
        {
            text.text = "Player's Won!" + "/" + player.hp.ToString() + "/" + chanceTurn.ToString();
            yield return new WaitForSeconds(5.0f);
            SceneManager.LoadScene(1);
        }

        playersTurn = true;
        enemiesMoving = false;
        StepTurn();
    }

    void Update()
    {
        if (playersTurn || playerMoving)
        {
            SetText();
            return;
        }
        else if (enemiesMoving)
        {
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
