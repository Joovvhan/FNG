using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{
    public int playerDamage = 1;
    //public int hp = 2;
    private GameManager gameManager;
    private BoardManager boardManager;
    private GameObject player;
    private int turnCount = 0;

    protected override void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddEnemyToList(this);

        boardManager = gameManager.GetComponent<BoardManager>();
        boardManager.AddEnemyToGrid((int)transform.position.x, this);
        player = GameObject.FindGameObjectWithTag("Player");
        hp = 2;
        isBlokcing = true;
        base.Start();
    }

    public void MoveEnemy()
    {
        if (turnCount % 2 == 0)
        {
            TryMoveForward();
        }
        else
        {
            TryMoveBackward();
        }
        turnCount += 1;
    }

    private bool TryMoveForward()
    {
        int dir = -1;
        int target_x = (int)transform.position.x + dir;
        if (boardManager.ApproveMovement(target_x))
        {
            Move(dir);
            boardManager.SetMovementGrid((int)transform.position.x, target_x);
            return true;
        }

        return false;
    }

    private bool TryMoveBackward()
    {
        int dir = 1;
        int target_x = (int)transform.position.x + dir;
        if (boardManager.ApproveMovement(target_x))
        {
            Move(dir);
            boardManager.SetMovementGrid((int)transform.position.x, target_x);
            return true;
        }

        return false;
    }

    private bool TryAttack()
    {
        List<int> attackPositions = new List<int>() { -1, 99, -1 };
        boardManager.SetDamage(attackPositions, 1);
        gameManager.playersTurn = false;
        gameManager.playerMoving = false;
        return true;
    }

}
