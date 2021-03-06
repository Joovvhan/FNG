using UnityEngine;
using System.Collections;

public abstract class Character : MonoBehaviour
{
    [SerializeField] protected float moveTime = 0.2f;
    public float hp = 3;
    private Rigidbody2D rb2D;           
    private float inverseMoveTime;
    [SerializeField] public bool isBlocking = false;

    protected virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    //protected bool Move(int xDir)
    protected IEnumerator Move(int xDir)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, 0);
        yield return StartCoroutine(SmoothMovement(end));
        //return true;
    }

    protected IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
            rb2D.MovePosition(newPostion);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    public abstract IEnumerator LoseHP(float damage);

    public float GetMoveTime()
    {
        return moveTime;
    }

    public bool IsDead()
    {
        return (hp <= 0);
    }

}