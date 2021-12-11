using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Player : Character
{
    public int wallDamage = 1;
    //public int hp = 3;
    private GameManager gameManager;
    private BoardManager boardManager;
    private int def = 0;

    protected override void Start()
    {
        base.Start();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        boardManager = gameManager.GetComponent<BoardManager>();
        gameManager.SetPlayer(this);
    }

    private void Awake()
    {
        CinemachineVirtualCamera cam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        cam.Follow = transform;
    }

    private void Update()
    {
        if (!gameManager.playersTurn || gameManager.playerMoving) return;

        int command = GetCommand();
        if (command != 0)
        {
            if (command == 1 || command == 2)
            {
                TryMove(command);
            }

            else if (command == 3)
            {
                TryAttack();
                Debug.Log("Attack");
            }

            else if(command == 4)
            {
                TryDefense();
                Debug.Log("Defense");
            }

            else if (command == 5)
            {
                TryExtra();
                Debug.Log("Extra");
            }
        }
    }

    private bool TryMove(int command)
    {
        int dir = 1;
        if (command == 2) {
            dir = -1;
        }

        int target_x = (int)transform.position.x + dir;
        if (boardManager.ApproveMovement(target_x))
        {
            gameManager.playerMoving = true;
            Move(dir);
            gameManager.playersTurn = false;
            gameManager.playerMoving = false;
            return true;
        }

        return false;
    }

    private bool TryAttack()
    {
        gameManager.playerMoving = true;
        List<int> attackPositions = new List<int>(){-1, 99, -1};
        boardManager.SetDamage(attackPositions, 1);
        gameManager.playersTurn = false;
        gameManager.playerMoving = false;
        return true;
    }

    private bool TryDefense()
    {
        gameManager.playerMoving = true;
        def = 3;
        gameManager.playersTurn = false;
        gameManager.playerMoving = false;
        return true;
    }

    private bool TryExtra()
    {
        gameManager.playerMoving = true;
        gameManager.playersTurn = false;
        gameManager.playerMoving = false;
        return true;
    }

    private int GetCommand()
    {
        //if (Input.GetKey(KeyCode.Space))
        //{
        //    return 0;
        //}

        if (Input.GetKey(KeyCode.RightArrow))
        {
            return 1;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            return 2;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            return 3;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            return 4;
        }
        else if (Input.GetKey(KeyCode.C))
        {
            return 5;
        }

        return 0;
    }

    //protected override void AttemptMove<T>(int xDir, int yDir)
    //{
    //    base.AttemptMove<T>(xDir, yDir);

    //    RaycastHit2D hit;
    //    Move(xDir, yDir, out hit);
    //    CheckIfGameOver();
    //    gameManager.playersTurn = false;
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", 1f);
            enabled = false;
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    private void CheckIfGameOver()
    {
        if (false)
        {
            gameManager.GameOver();
        }
    }

    new public void LoseHP(int damage)
    {
        int actual_damage = (int)Mathf.Clamp((def - damage), 0, 99);
        hp -= actual_damage;
        if (hp <= 0)
            gameObject.SetActive(false);
        Debug.Log("Player Lost Health");
    }

}