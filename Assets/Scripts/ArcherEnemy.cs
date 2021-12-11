using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : Enemy
{
    override public IEnumerator MoveEnemy()
    {
        SetDirection();
        if (turnCount % 2 == 0)
        {
            yield return StartCoroutine(PrepareAttack());
        }
        else if (turnCount % 2 == 1)
        {
            yield return StartCoroutine(RangeAttack());
        }
        turnCount += 1;
    }
}
