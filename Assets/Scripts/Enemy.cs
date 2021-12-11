using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Character
{
    //public int hp = 2;
    private GameManager gameManager;
    private BoardManager boardManager;
    private GameObject player;
    private int turnCount = 0;
    //private bool done = false;
    private int forward = -1;
    private int atk = 1;

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

        if (turnCount % 3 == 0)
        {
            yield return StartCoroutine(BasicAttack());
        }
        else if (turnCount % 3 == 1)
        {
            forward *= -1;
            yield return StartCoroutine(MoveAndMark(forward));
        }
        else if (turnCount % 3 == 2)
        {
            forward *= 1;
            yield return StartCoroutine(MoveAndMark(forward));
        }



        turnCount += 1;
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

    private IEnumerator BasicAttack()
    {
        List<int> attackPositions = new List<int> { (int)transform.position.x, (int)transform.position.x + forward };
        Debug.Log("Enemy Attacked");
        yield return new WaitForSeconds(1.0f);
        boardManager.SetPlayerDamage(attackPositions, atk);
        Debug.Log("Enemey Attack Finished");
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
