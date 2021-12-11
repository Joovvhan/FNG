using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    public int playerDamage = 1;
    //public int hp = 2;
    private Transform target;
    private bool skipMove;
    private GameManager gameManager;
    private BoardManager boardManager;

    protected override void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        boardManager = gameManager.GetComponent<BoardManager>();
        gameManager.AddEnemyToList(this);
        target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
        hp = 2;
    }

    public void MoveEnemy()
    {
        int xDir = 0;
        xDir = target.position.x > transform.position.x ? 1 : -1;
        //Debug.Log("Enemy do something");
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
