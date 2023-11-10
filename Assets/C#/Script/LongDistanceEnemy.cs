using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistanceEnemy : Enemy
{
    [SerializeField] private GameObject rock;
    [SerializeField] private float throwPower;
    [SerializeField] private Transform throwPos;

    public override IEnumerator Attack()
    {
        attackAni.SetTrigger("Attack");
        Rigidbody rockRigid = Instantiate(rock).GetComponent<Rigidbody>();
        rockRigid.transform.position = throwPos.transform.position;
        rockRigid.velocity = transform.forward * throwPower;
        m_enemy.enabled = false;
        isThrowing = true;
        yield return new WaitForSeconds(1);
        isThrowing = false;
        m_enemy.enabled = true;
    }
}