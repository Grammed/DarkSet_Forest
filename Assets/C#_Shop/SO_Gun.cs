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

	[Tooltip("좌우 반동")]
	/// <summary> 좌우 반동 </summary>
	public float recoilX = 1;

	[Tooltip("상하 반동")]
	/// <summary> 상하 반동 </summary>
	public float recoilY = 1; // 상하 반동

	[Tooltip("총 자체 앞뒤 반동")]
	/// <summary> 총 자체 앞뒤 반동 </summary>
	public float recoilZ = 1; // 앞뒤 반동

	[Tooltip("인체공학, 높을수록 반동 회복이 빠름")]
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
	public bool isClosedBolt = true; // 클로즈드 볼트
	#endregion

	#region Ammo

	[Header("Ammo")]
	public int maxAmmoInMag = 30; // 탄창 크기
	public int ammoInMag; // 현재 탄창 내에 있는 총알 개수
	[SerializeField]
	public int maxSpareAmmo; // 여분 총알의 최대 개수
	public int spareAmmo; // 현재 남은 여분 총알

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