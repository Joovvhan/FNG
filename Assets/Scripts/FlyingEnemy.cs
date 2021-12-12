using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;

public class FlyingEnemy : Enemy
{

    public override IEnumerator MoveEnemy()
    {
        if (isStunned)
        {
            yield return new WaitForSeconds(0.2f);
            isStunned = false;
        }
        else
        {
            SetDirection();
            if (IsPlayerInRange())
            {
                yield return StartCoroutine(BasicAttack());
            }
            else
            {
                yield return StartCoroutine(FlyAndMark(forward));
            }
        }
        turnCount += 1;
    }

    protected IEnumerator FlyAndMark(int xDir)
    {
        int old_x = (int)transform.position.x;
        int target_x = old_x + xDir;

        if (boardManager.ApproveFlying(target_x))
        {
            anim.SetBool("isRunning", true);
            yield return StartCoroutine(Move(xDir));
            anim.SetBool("isRunning", false);
        }
    }

}
