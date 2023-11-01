using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private string enemyType;//적 타입
    public string EnemyType { get { return enemyType; } }

    [SerializeField]
    private float hp;//적 체력
    public float Hp { get { return hp; }}

    [SerializeField]
    private float damage;// 적 공격력
    public float Damage { get { return damage; } }

    [SerializeField]
    private float sightRange;//적의 시야범위
    public float SightRange { get { return sightRange; } }

    [SerializeField]
    private float moveSpeed;//적의 이동속도
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField]
    private float range;//적의 공격사거리
    public float Range { get { return range; } }
}