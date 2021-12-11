using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //public static GameManager instance = null;
    private BoardManager boardScript;
    private float turnDelay = 0.5f;

    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public bool playerMoving = false;
    [HideInInspector] public bool enemiesMoving = false;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    private UnityEngine.UI.Text text;
    private Player player;
    private int chanceTurn = 0;
    private bool gameOver = false;

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


    public IEnumerator GameOver()
    {
        Debug.Log("Game Over");
        gameOver = true;
        string msg = "Player's Lost!" + "/" + player.hp.ToString() + "/" + chanceTurn.ToString();
        yield return StartCoroutine(Reload(msg));
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

        SetText();

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
            string msg = "Player's Won!" + "/" + player.hp.ToString() + "/" + chanceTurn.ToString();
            yield return StartCoroutine(Reload(msg));
        }

        playersTurn = true;
        enemiesMoving = false;
        StepTurn();
    }

    void Update()
    {
        if (!gameOver)
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

    private IEnumerator Reload(string msg)
    {
        text.text = msg;
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(1);
    }
}
