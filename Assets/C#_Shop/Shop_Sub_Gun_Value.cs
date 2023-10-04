using UnityEngine;

[CreateAssetMenu(fileName = "SubGun Data", menuName = "Scriptable Object/SubGun Data", order = int.MaxValue)]
public class Shop_Sub_Gun_Value : ScriptableObject
{
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