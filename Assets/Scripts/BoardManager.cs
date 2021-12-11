using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tile;
    public GameObject tree;
    public GameObject weakTree;
    public GameObject player;
    public GameObject enemy;
    private Transform boardHolder;
    private List<int> gridPositions = new List<int>();
    private List<Character> grid = new List<Character>();
    public int playerPosition = 0;
    public List<int> treePositions = new List<int>();
    public List<int> enemyPositions = new List<int>();

    public int columns = 8;
    //public int rows = 8;

    void InitialiseList()
    {
        gridPositions.Clear();
        grid.Clear();
        for (int x = 0; x < columns; x++)
        {
            gridPositions.Add(x);
            grid.Add(new Empty());
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

        GameObject instancePlayer = Instantiate(player, new Vector3(playerPosition, 0f, 0f), Quaternion.identity);
        instancePlayer.transform.SetParent(boardHolder);

        foreach (int v in treePositions)
        {
            GameObject instance = Instantiate(weakTree, new Vector3(v, 0, 0), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

        foreach (int v in enemyPositions)
        {
            GameObject instance = Instantiate(enemy, new Vector3(v, 0, 0), Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupScene()
    {
        InitialiseList();
        BoardSetup();
    }

    public bool ApproveMovement(int index)
    {
        //Debug.Log(index);
        //Debug.Log(grid.Count);
        if (index < 0 || index >= grid.Count)
        {
            return false;
        }
        else if (grid[index] == null)
        {
            return true;
        }
        else if(grid[index].isBlokcing)
        {
            return false;
        }

        return true;
    }
}
