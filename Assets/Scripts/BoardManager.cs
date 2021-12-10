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
    private List<Vector3> gridPositions = new List<Vector3>();
    public List<Vector3> treePositions = new List<Vector3>();
    public List<Vector3> enemyPositions = new List<Vector3>();

    public int columns = 8;
    public int rows = 8;

    void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++)
        {
            for (int y = 1; y < rows - 1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject instance = Instantiate(tile, new Vector3(x, y, 0f), Quaternion.identity);
                instance.transform.SetParent(boardHolder);
            }
        }

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    GameObject instance = Instantiate(tree, new Vector3(x, y, 0f), Quaternion.identity);
                    instance.transform.SetParent(boardHolder);
                }
            }
        }

        GameObject instancePlayer = Instantiate(player, new Vector3(0f, 0f, 0f), Quaternion.identity);
        instancePlayer.transform.SetParent(boardHolder);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupScene()
    {
        //treePositions.Add(new Vector3(1f, 1f, 0f));
        //treePositions.Add(new Vector3(2f, 1f, 0f));
        //treePositions.Add(new Vector3(3f, 1f, 0f));

        //enemyPositions.Add(new Vector3(3f, 3f, 0f));
        //enemyPositions.Add(new Vector3(3f, 5f, 0f));
        //enemyPositions.Add(new Vector3(5f, 3f, 0f));

        foreach (Vector3 v in treePositions)
        {
            GameObject instance = Instantiate(weakTree, v, Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }


        foreach (Vector3 v in enemyPositions)
        {
            GameObject instance = Instantiate(enemy, v, Quaternion.identity);
            instance.transform.SetParent(boardHolder);
        }

        BoardSetup();
        InitialiseList();
    }
}
