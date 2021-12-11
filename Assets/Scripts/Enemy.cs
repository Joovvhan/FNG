using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{
    public int playerDamage = 1;
    //public int hp = 2;
    private Transform target;
    private bool skipMove;
    private GameManager gameManager;
    private BoardManager boardManager;
    private int turnCount = 0;

    protected override void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddEnemyToList(this);

        boardManager = gameManager.GetComponent<BoardManager>();
        boardManager.AddEnemyToGrid((int)transform.position.x, this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
        hp = 2;
        isBlokcing = true;
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

    //protected override void AttemptMove<T>(int xDir, int yDir)
    //{
    //    if (skipMove)
    //    {
    //        skipMove = false;
    //        return;

    //    }
    //    base.AttemptMove<T>(xDir, yDir);
    //    skipMove = true;
    //}

    //public void MoveEnemy()
    //{
    //    int xDir = 0;
    //    int yDir = 0;

    //    if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
    //        yDir = target.position.y > transform.position.y ? 1 : -1;
    //    else
    //        xDir = target.position.x > transform.position.x ? 1 : -1;

    //    AttemptMove<Player>(xDir, yDir);
    //}

    //protected override void OnCantMove<T>(T component)
    //{
    //    Player hitPlayer = component as Player;
    //    hitPlayer.LoseHP(playerDamage);
    //    Debug.Log("Player Lost Health");
    //}

}
