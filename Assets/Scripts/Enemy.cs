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
    //private bool done = false;
    private int forward = -1;

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

    private void SetDirection()
    {
        if (player.transform.position.x <= transform.position.x)
        {
            forward = -1;
        }
        else
        {
            forward = 1;
        }
        transform.localScale = new Vector3(forward, 1, 1);
    }

    public IEnumerator MoveEnemy()
    {
        SetDirection();
        //done = false;
        if (turnCount % 2 == 1)
            forward *= -1;
        yield return StartCoroutine(MoveAndMark(forward));
        //{
        //    
        //}
        //else
        //{
        //    yield return StartCoroutine(MoveAndMark(+1));
        //}
        turnCount += 1;
        //done = true;
    }

    private bool CheckMove(int dir)
    {
        int target_x = (int)transform.position.x + dir;
        if (boardManager.ApproveMovement(target_x))
        {
            return true;
        }
        return false;
    }

    protected IEnumerator MoveAndMark(int xDir)
    {
        if (CheckMove(xDir))
        {
            int old_x = (int)transform.position.x;
            int target_x = old_x + xDir;
            yield return StartCoroutine(Move(xDir));
            boardManager.SetMovementGrid(old_x, target_x);
        }
    }

//private bool TryMoveForward()
//{
//    int dir = -1;
//    int target_x = (int)transform.position.x + dir;
//    if (boardManager.ApproveMovement(target_x))
//    {
//        Move(dir);
//        boardManager.SetMovementGrid((int)transform.position.x, target_x);
//        return true;
//    }

//    return false;
//}

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

    //public void SetDone(bool status)
    //{
    //    done = status;
    //}

    //public bool isDone()
    //{
    //    Debug.Log(done);
    //    return done;
    //}

    public override void LoseHP(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            boardManager.RemoveEnemyFromGrid((int)transform.position.x);
        }

        //Debug.Log("Lost Health");
    }

}
