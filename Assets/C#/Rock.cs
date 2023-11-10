using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private EnemyData enemyData;
    public EnemyData EnemyData { set { enemyData = value; } }

    private PlayerController playerController;

    private void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(DestroySelf());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && other.gameObject.tag == "Bunker")
        {
            playerController.Hit(enemyData.Damage);
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
    IEnumerator DestroySelf()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
    }
}
