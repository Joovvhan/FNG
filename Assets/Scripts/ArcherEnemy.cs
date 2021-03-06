using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    override public IEnumerator MoveEnemy()
    {
        if (isStunned)
        {
            yield return new WaitForSeconds(0.2f);
            isStunned = false;
        }
        else
        {
            SetDirection();
            if (turnCount % 3 == 0)
            {
                yield return StartCoroutine(DoNothing());
            }
            else if (turnCount % 3 == 1)
            {
                yield return StartCoroutine(PrepareAttack());
            }
            else if (turnCount % 3 == 2)
            {
                yield return StartCoroutine(RangeAttack());
            }
            turnCount += 1;
        }
    }
}