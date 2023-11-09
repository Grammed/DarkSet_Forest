using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealer : Enemy
{
    bool canHeal = false;
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && canHeal)
        {
            other.GetComponent<Enemy>().enemyHp += 10;
        }
    }
    public IEnumerator Heal()
    {
        while (true)
        {
            float time = 0;
            if (time > 10f)
            {
                canHeal = true;
                yield return new WaitForSeconds(1f);
                canHeal = false;
                time = 0;
            }
            time += Time.deltaTime;
        }
    }
}
