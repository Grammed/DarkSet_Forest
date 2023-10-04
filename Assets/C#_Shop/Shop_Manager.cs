using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Manager : MonoBehaviour
{
    [Header("ÄÚÀÎ")]
    public int Coin;
    public Text Coin_Text;
    public GameObject Dontmoney;

    [Header("Gun_info"),SerializeField]
    private List<Shop_Main_Gun_Value> shop_Main_Gun_Value;
    [SerializeField]
    private List<Shop_Sub_Gun_Value> shop_Sub_Gun_Value;

    [Header("ÃÑ±â °ü·Ã")]
    public Image MainGun;
    public Image SubGun;
    public Text MainGuntext;
    public Text SubGuntext;
    public Text MainGunCost;
    public Text SubGunCost;

    [Header("ÃÑ±â Panel")]
    public GameObject MainGun_Panel;
    public GameObject SubGun_Panel;

    [Header("ºÎ¼Ó")]
    public Text Bullettext;
    public int Bullet;
    public Text HPtext;
    //public PlayerController PC;

    private int GunIndex;

    private bool isMainGun;

    void Start() // OnEnable·Î ¹Ù²ã¾ßÇÔ SetAcitveÇÒ·Á¸é
    {
        MainGun.sprite = shop_Main_Gun_Value[0].GunImage;
        SubGun.sprite = shop_Sub_Gun_Value[0].GunImage;
        isMainGun = true;
        MainGun_Panel.SetActive(true);
        SubGun_Panel.SetActive(false);
        Bullettext.text = Bullet.ToString();
        //HPtext.text = HP.ToString();
        Dontmoney.SetActive(false);


        Bullet = 0; //¾ø¾Ú
        Coin = 1000000; // ¾ø¾Ú
    }

    private void Update()
    {
        Coin_Text.text = Coin.ToString();
        Bullettext.text = "ÃÑ¾Ë °¹¼ö : " + Bullet.ToString() + "°³";
        //HPtext.text = "ÇöÀç Ã¼·Â : " + currentHP.ToString() + "HP";
    }

    public void Main_Weapon_Select()
    {
        isMainGun = true;
        MainGun_Panel.SetActive(true);
        SubGun_Panel.SetActive(false);
        GunIndex = 0;
        MainGun.sprite = shop_Main_Gun_Value[GunIndex].GunImage;
        MainGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
        MainGunCost.text = shop_Main_Gun_Value[GunIndex].Cost.ToString() + " Coin";
    }

    public void Sub_Weapon_Select()
    {
        isMainGun = false;
        MainGun_Panel.SetActive(false);
        SubGun_Panel.SetActive(true);
        GunIndex = 0;
        SubGun.sprite = shop_Sub_Gun_Value[GunIndex].GunImage;
        SubGuntext.text = shop_Sub_Gun_Value[GunIndex].GunName;
        SubGunCost.text = shop_Sub_Gun_Value[GunIndex].Cost.ToString() + " Coin";
    }

    public void MainGun_Buy()
    {
        if (Coin >= shop_Main_Gun_Value[GunIndex].Cost)
        {
            Coin -= shop_Main_Gun_Value[GunIndex].Cost;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void SubGun_Buy()
    {
        if (Coin >= shop_Sub_Gun_Value[GunIndex].Cost)
        {
            Coin -= shop_Sub_Gun_Value[GunIndex].Cost;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void Bullet_Buy()
    {
        if (Coin >= 500)
        {
            Coin -= 500;
            Bullet += 90;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void Healing_Buy()
    {
        if (Coin >= 500)
        {
            Coin -= 1000;
            Debug.Log("Ã¼·ÂÈ¸º¹");
            //Hp += 50;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void Gun_Change_Plus()
    {
        if (isMainGun == true)
        {
            if (GunIndex < shop_Main_Gun_Value.Count)
            {
                GunIndex++;
                MainGun.sprite = shop_Main_Gun_Value[GunIndex].GunImage;
                MainGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
            }
        }
        if(isMainGun == false) 
        {
            if (GunIndex < shop_Sub_Gun_Value.Count)
            {
                GunIndex++;
                SubGun.sprite = shop_Sub_Gun_Value[GunIndex].GunImage;
                SubGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
            }
        }
    }

    public void Gun_Change_Minus()
    {
        if (isMainGun == true)
        {
            if (GunIndex > 0)
            {
                GunIndex--;
                MainGun.sprite = shop_Main_Gun_Value[GunIndex].GunImage;
                MainGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
            }
        }
        if (isMainGun == false)
        {
            if (GunIndex > 0)
            {
                GunIndex--;
                SubGun.sprite = shop_Sub_Gun_Value[GunIndex].GunImage;
                SubGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
            }
        }
    }

    public IEnumerator NotEnoughMoney()
    {
        Dontmoney.SetActive(true);
        yield return new WaitForSeconds(1f);
        Dontmoney.SetActive(false);
    }
}
