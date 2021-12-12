using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tile;
    public GameObject tree;
    public GameObject weakTree;
    public GameObject playerPrefab;
    public GameObject warriorPrefab;
    public GameObject archerPrefab;
    public GameObject archerPrefab1;
    public GameObject batPrefab;
    private Transform boardHolder;

    public GameObject testPrefab;

    //private List<int> gridPositions = new List<int>();
    [SerializeField] private List<Enemy> grid = new List<Enemy>();
    [SerializeField] private List<FlyingEnemy> flyingEnemies = new List<FlyingEnemy>();

    public int playerPosition = 0;
    public List<int> treePositions = new List<int>();
    public List<int> warriorPositions = new List<int>();
    public List<int> archerPositions = new List<int>();
    public List<int> archer1Positions = new List<int>();
    public List<int> batPositions = new List<int>();

    public List<int> testPositions = new List<int>();

    private Player player;

    private GameManager gameManager;

    public int columns = 8;
    //public int rows = 8;

    void InitialiseList()
    {
        //gridPositions.Clear();
        grid.Clear();
        flyingEnemies.Clear();
        for (int x = 0; x < columns; x++)
        {
            //gridPositions.Add(x);
            //grid.Add(new Empty());
            grid.Add(null);
        }

    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = 0; x < columns; x++)
        {
            GameObject instance = Instantiate(tile, new Vector3(x, 0, 0f), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

        GameObject instancePlayer = Instantiate(playerPrefab, new Vector3(playerPosition, 0f, 0f), Quaternion.identity);
        instancePlayer.transform.SetParent(boardHolder);

        foreach (int v in treePositions)
        {
            GameObject instance = Instantiate(weakTree, new Vector3(v, 0, 0), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

        foreach (int v in warriorPositions)
        {
            GameObject instance = Instantiate(warriorPrefab, new Vector3(v, 0, 0), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

        foreach (int v in archerPositions)
        {
            GameObject instance = Instantiate(archerPrefab, new Vector3(v, 0, 0), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

        foreach (int v in archer1Positions)
        {
            GameObject instance = Instantiate(archerPrefab1, new Vector3(v, 0, 0), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

        foreach (int v in batPositions)
        {
            GameObject instance = Instantiate(batPrefab, new Vector3(v, 0, 0), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

        foreach (int v in testPositions)
        {
            GameObject instance = Instantiate(testPrefab, new Vector3(v, 0, 0), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

    }

    public void SetGameManager(GameManager manager)
    {
        gameManager = manager;
    }

    public bool AddEnemyToGrid(int idx, Enemy script)
    {
        if (script is FlyingEnemy)
        {
            AddFlyingEnemies(script as FlyingEnemy);
            return true;
        }
        else
        {
            if (idx < 0 || idx > grid.Count)
            {
                return false;
            }
            else
            {
                grid[idx] = script;
            }
            return true;
        }
    }

    public bool RemoveEnemyFromGrid(int idx)
    {
        if (idx < 0 || idx > grid.Count)
        {
            return false;
        }
        else
        {
            grid[idx] = null;
        }
        return true;
    }

    public void SetupScene()
    {
        InitialiseList();
        BoardSetup();
    }

    public bool ApproveMovement(int index)
    {
        if (index < 0 || index >= grid.Count)
        {
            return false;
        }
        else if (grid[index] == null)
        {
            return true;
        }
        else if(grid[index].isBlocking)
        {
            return false;
        }

        return true;
    }

    public bool ApproveMovementGhostMode(int index)
    {
        if (index < 0 || index >= grid.Count)
        {
            return false;
        }
        else if (grid[index] == null)
        {
            return true;
        }

        return true;
    }

    public bool ApproveBlockerMovement(int index)
    {
        if ((int)player.transform.position.x == index)
        {
            return false;
        } 

        return true;
    }

    public void AddFlyingEnemies(FlyingEnemy enemy)
    {
        flyingEnemies.Add(enemy);
    }

    public bool ApproveFlying(int index)
    {
        if (index < 0 || index >= grid.Count)
        {
            return false;
        }

        return true;
    }

    public bool SetMovementGrid(int prev, int next)
    {
        var unit = grid[prev];
        grid[prev] = grid[next];
        grid[next] = unit;

        return true;
    }

    public IEnumerator SetDamage(List<int> indices, int damage)
    {
        foreach (int index in indices)
        {
            if (index < 0 || index >= grid.Count)
            {
                
            }
            else if (grid[index] == null)
            {
                
            }
            else
            {
                yield return StartCoroutine(grid[index].LoseHP(damage));
                if (gameManager.IsChance())
                {
                    grid[index].SetStunned();
                }
            }

            foreach (FlyingEnemy bat in flyingEnemies)
            {
                if (index == bat.transform.position.x & bat.isActiveAndEnabled)
                {
                    Debug.Log("Bat is damaged");
                    yield return StartCoroutine(bat.LoseHP(damage));
                    if (gameManager.IsChance())
                    {
                        bat.SetStunned();
                    }
                }
            }
        }
    }

    public IEnumerator SetPlayerDamage(List<int> indices, int damage, Enemy enemy)
    {
        foreach (int idx in indices)
        {
            if (idx == (int)player.transform.position.x) {
                yield return StartCoroutine(player.LoseHP(damage));

                if (gameManager.IsChance() & player.IsDefense())
                {
                    Debug.Log("Countered Melee Attack");
                    yield return StartCoroutine(player.PlayCounter());
                    yield return StartCoroutine(enemy.LoseHP(damage));
                }
                yield break;
            }
        }
    }

    public IEnumerator SetPlayerDamage(int damage, Enemy enemy)
    {
        yield return StartCoroutine(player.LoseHP(damage));
        if (gameManager.IsChance() & player.IsDefense())
        {
            Debug.Log("Countered Range Attack");
            yield return StartCoroutine(player.PlayCounter());
            yield return StartCoroutine(enemy.LoseHP(damage));
        }
    }

    public void SetPlayer(Player p)
    {
        player = p;
    }

    public int GetPlayerPosition()
    {
        return (int)player.transform.position.x;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
