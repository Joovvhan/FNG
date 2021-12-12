using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //public static GameManager instance = null;
    private BoardManager boardScript;
    private float turnDelay = 0.5f;

    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public bool playerMoving = false;
    [HideInInspector] public bool enemiesMoving = false;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] Text statusText = null;
    [SerializeField] TextMeshProUGUI chanceText = null;
    private Player player;
    private int chanceTurn = 1;
    private bool gameOver = false;

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
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
        string msg = "Player's Lost!";
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
            string msg = "Player's Won!";
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
        string chanceTurnString = chanceTurn.ToString();
        if(chanceTurn == 0)
        {
            chanceTurnString = "CHANCE!";
        }

        if (playerMoving)
        {
            statusText.text = "- PLAYER MOVING -";
            chanceText.text = chanceTurnString;
        }
        else if (playersTurn)
        {
            statusText.text = "- PLAYER TURN -";
            chanceText.text = chanceTurnString;
        }
        else if (enemiesMoving)
        {
            statusText.text = "- ENEMY TURN -";
            chanceText.text = chanceTurnString;
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
        chanceText.text = msg;
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(1);
    }
}
