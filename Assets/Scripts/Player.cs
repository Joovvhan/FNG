using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Cinemachine;

public class Player : Character
{
    public int wallDamage = 1;
    //public int hp = 3;
    private GameManager gameManager;
    private BoardManager boardManager;

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

        int horizontal = 0;
        horizontal = (int)(Input.GetAxisRaw("Horizontal"));

        if (horizontal != 0)
        {
            int target_x = (int)transform.position.x + horizontal;
            if (boardManager.ApproveMovement(target_x))
            {
                gameManager.playerMoving = true;
                Move(horizontal);
                //Debug.Log("Move IEnumerator Finshed");
                gameManager.playersTurn = false;
                gameManager.playerMoving = false;
                //AttemptMove<Wall>(horizontal, vertical);
            }

        }
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

}