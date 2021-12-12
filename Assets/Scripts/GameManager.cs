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
    //private float turnDelay = 0.5f;
    private float turnDelay = 1.0f;

    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public bool playerMoving = false;
    [HideInInspector] public bool enemiesMoving = false;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();

    [SerializeField] Text statusText = null;
    [SerializeField] TextMeshProUGUI chanceText = null;
    [SerializeField] Text chanceCountText = null;
    [SerializeField] public float playerHP;
    //private UnityEngine.UI.Text text;

    public int chanceCount = 1;
    public int chanceTurnCount = 4;

    private Player player;
    private int chanceTurn = 1;
    //private int chanceTurn = 0;
    private bool gameOver = false;

    void Awake()
    {
        boardScript = GetComponent<BoardManager>();
        boardScript.SetGameManager(this);
        //text = GameObject.Find("Text").GetComponent<UnityEngine.UI.Text>();

        InitGame();
    }

    void InitGame()
    {
        boardScript.SetupScene();
    }


    public IEnumerator GameOver(string msg)
    {
        Debug.Log("Game Over");
        gameOver = true;
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
        yield return new WaitForSeconds(0.2f);
        player.SetOutline(false);
        yield return new WaitForSeconds(turnDelay);

        SetText();

        ClearDeadEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
            {
                enemies[i].SetOutline(true);
                yield return new WaitForSeconds(0.2f);
                yield return StartCoroutine(enemies[i].MoveEnemy());
                yield return new WaitForSeconds(0.2f);
                enemies[i].SetOutline(false);
            }
        }

        ClearDeadEnemies();

        if (IsGameCleared())
        {
            string msg = "Player's Won!" + "/" + player.hp.ToString() + "/" + chanceTurn.ToString();
            yield return StartCoroutine(GameOver(msg));

        }

        if (IsPlayerDefeated())
        {
            string msg = "Player's Lost!" + "/" + player.hp.ToString() + "/" + chanceTurn.ToString();
            yield return StartCoroutine(GameOver(msg));
        }


        yield return new WaitForSeconds(turnDelay);

        playersTurn = true;
        enemiesMoving = false;
        player.SetOutline(true);
        StepTurn();
    }

    void Update()
    {
        if (chanceCount > 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (chanceTurn == 0)
                {
                    chanceTurn = 1;
                }
                else
                {
                    chanceTurn = 0;
                }
            }
        }

        chanceCountText.text = "Chance: " + chanceCount;

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

        string chanceTurnString = "";
        if (chanceTurn == 0)
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

    private void StepTurn()
    {
        // chanceTurn = (chanceTurn + 1) % chanceTurnCount;
        // chanceTurn += 1;

        if (IsChance())
        {
            chanceCount = Mathf.Clamp(chanceCount - 1, 0, chanceCount);
        }
        chanceTurn = 1;
    }

    public bool IsChance()
    {
        return (chanceTurn % chanceTurnCount == 0);
    }

    private IEnumerator Reload(string msg)
    {
        statusText.text = msg;
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene(1);
    }

    private void ClearDeadEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
            {
                if (enemies[i].IsDead())
                {
                    enemies[i].gameObject.SetActive(false);
                    if (!(enemies[i] is FlyingEnemy))
                    {
                        boardScript.RemoveEnemyFromGrid((int)enemies[i].transform.position.x);
                    }
                }
            }
        }
    }

    private bool IsGameCleared()
    {
        for (int i = 0; i < enemies.Count; i++)
            if (enemies[i].isActiveAndEnabled)
                return false;

        return true;
    }

    private bool IsPlayerDefeated()
    {
        if (player.IsDead())
            return true;
        return false;
    }
}
