using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeEnemy : Enemy
{

    public override IEnumerator Attack()
    {
        attackAni.SetTrigger("Attack");
        yield return new WaitForSeconds(1f);
        SetTarget();
    }
}
