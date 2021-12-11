using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraversingEnemy : Enemy
{
    override public IEnumerator MoveEnemy()
    {
        SetDirection();
        if (turnCount % 2 == 0)
        {
            forward *= -1;
            yield return StartCoroutine(MoveAndMark(forward));
        }
        else if (turnCount % 2 == 1)
        {
            forward *= 1;
            yield return StartCoroutine(MoveAndMark(forward));
        }
        turnCount += 1;
    }
}
