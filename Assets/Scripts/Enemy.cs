using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;

public class Enemy : Character
{
    //public int hp = 2;
    protected GameManager gameManager;
    protected BoardManager boardManager;
    protected GameObject player;
    protected int turnCount = 0;
    //private bool done = false;
    protected int forward = -1;
    protected int atk = 1;
    Animator anim;
    [SerializeField] protected MMFeedbacks damageFeedback;
    protected bool isStunned;

    protected override void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.AddEnemyToList(this);

        boardManager = gameManager.GetComponent<BoardManager>();
        boardManager.AddEnemyToGrid((int)transform.position.x, this);
        player = GameObject.FindGameObjectWithTag("Player");
        //hp = 2;
        //isBlocking = true;
        base.Start();
        anim = GetComponent<Animator>();
        isStunned = false;
    }

    protected void SetDirection()
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

    public virtual IEnumerator MoveEnemy()
    {
        if (isStunned)
        {
            yield return new WaitForSeconds(0.2f);
            isStunned = false;
        }
        else
        {
            SetDirection();

            if (turnCount % 2 == 0)
            {
                yield return StartCoroutine(BasicAttack());
            }
            else if (turnCount % 2 == 1)
            {
                yield return StartCoroutine(DoNothing());
            }
        }
        turnCount += 1;
    }

    private bool CheckMove(int dir)
    {
        int target_x = (int)transform.position.x + dir;

        if (boardManager.ApproveMovement(target_x))
        {
            if (!isBlocking)
            {
                return true;
            }
            else if (boardManager.ApproveBlockerMovement(target_x))
            {
                return true;
            }
            
        }
        return false;
    }

    protected IEnumerator MoveAndMark(int xDir)
    {
        if (CheckMove(xDir))
        {
            int old_x = (int)transform.position.x;
            int target_x = old_x + xDir;
            anim.SetBool("isRunning", true);
            yield return StartCoroutine(Move(xDir));
            anim.SetBool("isRunning", false);
            boardManager.SetMovementGrid(old_x, target_x);
        }
    }

    protected IEnumerator BasicAttack()
    {
        List<int> attackPositions = new List<int> { (int)transform.position.x, (int)transform.position.x + forward };
        //Debug.Log("Enemy Attacked");
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("Idle");
        yield return StartCoroutine(boardManager.SetPlayerDamage(attackPositions, atk, this));
        //Debug.Log("Enemey Attack Finished");
    }

    public override IEnumerator LoseHP(int damage)
    {
        hp -= damage;
        yield return StartCoroutine(damageFeedback.PlayFeedbacksCoroutine(this.transform.position, 1.0f, false));
        if (hp <= 0)
        {
            anim.SetTrigger("Die");
            yield return new WaitForSeconds(0.5f);
            //gameObject.SetActive(false);
            //boardManager.RemoveEnemyFromGrid((int)transform.position.x);
        }

        //Debug.Log("Lost Health");
    }

    protected IEnumerator PrepareAttack()
    {
        anim.SetTrigger("Draw");
        yield return new WaitForSeconds(0.8f);
    }
    protected IEnumerator DoNothing()
    {
        yield return new WaitForSeconds(0.8f);
    }

    protected IEnumerator RangeAttack()
    {
        anim.SetTrigger("Launch");
        yield return new WaitForSeconds(0.8f);
        yield return StartCoroutine(boardManager.SetPlayerDamage(atk, this));
    }

    public void SetStunned()
    {
        isStunned = true;
        Debug.Log("An enemy is stunned");
    }

}
