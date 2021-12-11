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
    private int atk = 1;
    private int forward = 1;
    Animator anim;
    //[SerializeField] private GameObject sprite;

    protected override void Start()
    {
        base.Start();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        boardManager = gameManager.GetComponent<BoardManager>();
        boardManager.SetPlayer(this);
        gameManager.SetPlayer(this);
        anim = GetComponent<Animator>();
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
            def = 0;
            if (command == 1 || command == 2)
            {
                StartCoroutine(TryMove(command));
            }

            else if (command == 3)
            {
                StartCoroutine(BasicAttack());
            }

            else if(command == 4)
            {
                StartCoroutine(TryDefense());
                Debug.Log("Defense");
            }

            else if (command == 5)
            {
                StartCoroutine(TryBackStep());
            }

            else if (command == 6)
            {
                Debug.Log("Skip!");
                gameManager.playersTurn = false;
            }
        }
    }

    private IEnumerator TryMove(int command)
    {
        int dir = 1;
        if (command == 2) {
            dir = -1;
        }

        forward = dir;
        //sprite.transform.localScale = new Vector3(dir, 1, 1);
        transform.localScale = new Vector3(dir, 1, 1);

        int target_x = (int)transform.position.x + dir;
        if (boardManager.ApproveMovement(target_x))
        {
            gameManager.playerMoving = true;
            anim.SetBool("isRunning", true);
            yield return StartCoroutine(Move(dir));
            anim.SetBool("isRunning", false);
            gameManager.playersTurn = false;
            gameManager.playerMoving = false;
        }

    }

    private IEnumerator BasicAttack()
    {
        List<int> attackPositions = new List<int> { (int)transform.position.x, (int)transform.position.x + forward };
        gameManager.playerMoving = true;
        Debug.Log("Player Attack");
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(boardManager.SetDamage(attackPositions, atk));
        anim.SetTrigger("Idle");
        gameManager.playersTurn = false;
        gameManager.playerMoving = false;
        Debug.Log("Player Attack Finished");
    }

    private IEnumerator TryDefense()
    {
        gameManager.playerMoving = true;
        def = 3;
        yield return new WaitForSeconds(1.0f);
        gameManager.playersTurn = false;
        gameManager.playerMoving = false;
    }

    private IEnumerator TryBackStep()
    {
        int dir = -1 * forward;
        int target_x = (int)transform.position.x + dir;
        Debug.Log("TryBackStep" + " " + dir.ToString() + " " + target_x.ToString());
        if (boardManager.ApproveMovement(target_x))
        {
            gameManager.playerMoving = true;
            yield return StartCoroutine(Move(dir));
            gameManager.playersTurn = false;
            gameManager.playerMoving = false;
        }
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
        else if (Input.GetKey(KeyCode.Space))
        {
            return 6;
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

    public override IEnumerator LoseHP(int damage)
    {
        int actual_damage = (int)Mathf.Clamp((damage - def), 0, 99);
        hp -= actual_damage;
        Debug.Log("Player Lost Health");
        if (hp <= 0)
        {
            anim.SetTrigger("Die");
            StartCoroutine(gameManager.GameOver());
            yield return new WaitForSeconds(1.0f);
            gameObject.SetActive(false);
        }
    }

}