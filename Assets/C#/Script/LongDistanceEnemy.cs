using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongDistanceEnemy : Enemy
{
    [SerializeField] private GameObject rock;
    [SerializeField] private float throwPower;

    public override IEnumerator Attack()
    {
        attackAni.SetTrigger("Attack");
        yield return new WaitForSeconds(1);
        Instantiate(rock);
        rock.transform.position = transform.position;
        var rockRigid = rock.GetComponent<Rigidbody>();
        rockRigid.AddForce(Vector3.forward * throwPower, ForceMode.Impulse);
    }
}