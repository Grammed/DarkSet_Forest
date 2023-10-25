using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gun Data", menuName = "Scriptable Object/Gun Data", order = int.MaxValue)]
public class SO_Gun : ScriptableObject
{
	public string gunName;
	public GunType type;
	public GameObject prefab;

	#region fire
	[Header("Fire")]

	public float gunDamage = 25;

	public float fireTime = 0.2f;

	public bool isAutomatic;

	public int hitGold;

	#endregion

	#region Recoil

	[Header("Recoil")]

	[Tooltip("�¿� �ݵ�")]
	/// <summary> �¿� �ݵ� </summary>
	public float recoilX = 1;

	[Tooltip("���� �ݵ�")]
	/// <summary> ���� �ݵ� </summary>
	public float recoilY = 1; // ���� �ݵ�

	[Tooltip("�� ��ü �յ� �ݵ�")]
	/// <summary> �� ��ü �յ� �ݵ� </summary>
	public float recoilZ = 1; // �յ� �ݵ�

	[Tooltip("��ü����, �������� �ݵ� ȸ���� ����")]
	public float ergonomic = 70;


	#endregion

	#region Sound

	[Header("Sound")]
	public AudioClip reloadSound;
	public AudioClip fireSound;
	#endregion

	#region Reload

	[Header("Reload")]
	public float reloadTime = 3f;
	public bool isClosedBolt = true; // Ŭ����� ��Ʈ
	#endregion

	#region Ammo

	[Header("Ammo")]
	public int maxAmmoInMag = 30; // źâ ũ��
	public int ammoInMag; // ���� źâ ���� �ִ� �Ѿ� ����
	[SerializeField]
	public int maxSpareAmmo; // ���� �Ѿ��� �ִ� ����
	public int spareAmmo; // ���� ���� ���� �Ѿ�

	#endregion

	[Header("Gun Value")]
    [SerializeField]
    private Sprite gunImage;
    public Sprite GunImage { get { return gunImage; } }
    public string GunName { get { return gunName; } }

    [SerializeField]
    private int cost;
    public int Cost { get { return cost; } }
}

public enum GunType
{
	Primary, // Main
	Secondary, // Sub

}