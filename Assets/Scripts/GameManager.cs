using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int currentStage;
    //public static GameManager instance = null;
    private BoardManager boardScript;
    //private float turnDelay = 0.5f;
    //private float turnDelay = 1.0f;
    private float turnDelay = 0.2f;

    [HideInInspector] public bool playersTurn = true;
    [HideInInspector] public bool playerMoving = false;
    [HideInInspector] public bool enemiesMoving = false;
    [SerializeField] private List<Enemy> enemies = new List<Enemy>();
    [SerializeField] private List<Trap> traps = new List<Trap>();

    [SerializeField] Text statusText = null;
    [SerializeField] TextMeshProUGUI chanceText = null;
    [SerializeField] public float playerHP;
    //private UnityEngine.UI.Text text;

    public int chanceCount = 1;
    public int chanceTurnCount = 4;
    public bool isActiveChanceUI = false;

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

    public IEnumerator GameClear(string msg)
    {
        Debug.Log("Game Clear");
        gameOver = true;
        yield return StartCoroutine(ReloadNext(msg));
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
        yield return new WaitForSeconds(0.1f);
        player.SetOutline(false);
        yield return new WaitForSeconds(turnDelay);

        SetText();

        ClearDeadEnemies();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].isActiveAndEnabled)
            {
                enemies[i].SetOutline(true);
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(enemies[i].MoveEnemy());
                yield return new WaitForSeconds(0.1f);
                enemies[i].SetOutline(false);
            }
        }

        for (int i = 0; i < traps.Count; i++)
        {
            yield return new WaitForSeconds(0.1f);
            yield return StartCoroutine(traps[i].SetNeedle());
            yield return new WaitForSeconds(0.1f);
        }

        ClearDeadEnemies();

        if (IsGameCleared())
        {
            string msg = "YOU WIN!";
            //yield return StartCoroutine(GameOver(msg));
            yield return StartCoroutine(GameClear(msg));

        }

        if (IsPlayerDefeated())
        {
            string msg = "YOU DIE...";
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
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(currentStage);
        }

        if (!gameOver)
        {
            if (playersTurn || playerMoving)
            {
                if (chanceCount > 0)
                {
                    if (Input.GetKeyDown(KeyCode.A))
                    {
                        if (chanceTurn == 0)
                        {
                            chanceTurn = 1;
                            isActiveChanceUI = false;
                        }
                        else
                        {
                            chanceTurn = 0;
                            isActiveChanceUI = true;
                        }
                    }
                }
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
            statusText.text = "YOUR TURN";
        }
        else if (playersTurn)
        {
            statusText.text = "YOUR TURN";
        }
        else if (enemiesMoving)
        {
            statusText.text = "ENEMY TURN";
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
        chanceTurn = 1;
    }

    public void UseChanceCount()
    {
        if (IsChance())
        {
            chanceCount = Mathf.Clamp(chanceCount - 1, 0, chanceCount);
        }
    }

    public bool IsChance()
    {
        return (chanceTurn % chanceTurnCount == 0);
    }

    private IEnumerator Reload(string msg)
    {
        statusText.text = msg;
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(currentStage);
    }

    private IEnumerator ReloadNext(string msg)
    {
        statusText.text = msg;
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene(currentStage + 1);
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

    public void AddItem()
    {
        chanceCount += 1;
        FindObjectOfType<ChanceUI>().SwitchChanceUI(true);
        Debug.Log("Chance Count Up! (" + chanceCount.ToString() + ")");
    }

    public void AddTrapToList(Trap script)
    {
        traps.Add(script);
    }

    public int GetPlayerPosition()
    {
        return (int)player.transform.position.x;
    }
}
