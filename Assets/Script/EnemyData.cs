using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Data", menuName = "Scriptable Object/Enemy Data", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    [SerializeField]
    private string enemyType;//�� Ÿ��
    public string EnemyType { get { return enemyType; } }

    [SerializeField]
    private float hp;//�� ü��
    public float Hp { get { return hp; }}

    [SerializeField]
    private float damage;// �� ���ݷ�
    public float Damage { get { return damage; } }

    [SerializeField]
    private float sightRange;//���� �þ߹���
    public float SightRange { get { return sightRange; } }

    [SerializeField]
    private float moveSpeed;//���� �̵��ӵ�
    public float MoveSpeed { get { return moveSpeed; } }

    [SerializeField]
    private float range;//���� ���ݻ�Ÿ�
    public float Range { get { return range; } }
}