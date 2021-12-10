using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MovingObject
{
    //Delay time in seconds to restart level.
    public float restartLevelDelay = 1f;
    //How much damage a player does to a wall whne chopping it.
    public int wallDamage = 1;
    //public int hp = 3;
    private GameManager gameManager;

    protected override void Start()
    {
        base.Start();
        hp = 3;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        //If it's not the player's turn, exit the function.
        if (!gameManager.playersTurn) return;

        int horizontal = 0;      //Used to store the horizontal move direction.
        int vertical = 0;        //Used to store the vertical move direction.

        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;
        Move(xDir, yDir, out hit);
        CheckIfGameOver();
        gameManager.playersTurn = false;
    }

    //protected override void OnCantMove<T>(T component)
    //{
    //    Wall hitWall = component as Wall;
    //    hitWall.LoseHP(wallDamage);
    //}

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.LoseHP(wallDamage);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
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

    //public void LoseHP(int damage)
    //{
    //    hp -= damage;
    //}
}