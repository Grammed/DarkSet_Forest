using UnityEngine;

[CreateAssetMenu(fileName = "MainGun Data", menuName = "Scriptable Object/MainGun Data", order = int.MaxValue)]
public class Shop_Main_Gun_Value : ScriptableObject
{
    [Header("Main Gun Value")]
    [SerializeField]
    private Sprite gunImage;
    public Sprite GunImage { get { return gunImage; } }

    [SerializeField]
    private string gunName;
    public string GunName { get { return gunName; } }

    [SerializeField]
    private int cost;
    public int Cost { get { return cost; } }
}