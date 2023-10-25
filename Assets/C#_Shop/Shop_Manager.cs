using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Manager : MonoBehaviour
{
    [Header("¹«±â ¸Å´ÏÀú")]
    WeaponManager weaponManager;

    [Header("ÃÑ")]
    public Gun gunScript;
    [Header("Gun_info"),SerializeField]
    private List<SO_Gun> shop_Main_Gun_Value;
    [SerializeField]
    private List<SO_Gun> shop_Sub_Gun_Value;

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

    [SerializeField]
    private MoneyManager moneyManager;

    void OnEnable() // OnEnable·Î ¹Ù²ã¾ßÇÔ SetAcitveÇÒ·Á¸é
    {
        weaponManager = FindAnyObjectByType<WeaponManager>();
        moneyManager = FindObjectOfType<MoneyManager>();
        

        MainGun.sprite = shop_Main_Gun_Value[0].GunImage;
        SubGun.sprite = shop_Sub_Gun_Value[0].GunImage;
        isMainGun = true;
        MainGun_Panel.SetActive(true);
        SubGun_Panel.SetActive(false);
        Bullettext.text = Bullet.ToString();
        //HPtext.text = HP.ToString();
        moneyManager.Dontmoney.SetActive(false);


        Bullet = gunScript.spareAmmo; //¾ø¾Ú
        moneyManager.Coin = 1000000; // ¾ø¾Ú
    }

    private void Update()
    {
        moneyManager.Coin_Text.text = moneyManager.Coin.ToString();
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
        if (moneyManager.Coin >= shop_Main_Gun_Value[GunIndex].Cost)
        {
            moneyManager.Coin -= shop_Main_Gun_Value[GunIndex].Cost;
            weaponManager.ChangePrimary(shop_Main_Gun_Value[GunIndex].prefab);
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void SubGun_Buy()
    {
        if (moneyManager.Coin >= shop_Sub_Gun_Value[GunIndex].Cost)
        {
            moneyManager.Coin -= shop_Sub_Gun_Value[GunIndex].Cost;
            weaponManager.ChangeSecondary(shop_Sub_Gun_Value[GunIndex].prefab);
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void Bullet_Buy()
    {
        if (moneyManager.Coin >= 500 && gunScript.spareAmmo <= gunScript.maxSpareAmmo)
        {
            moneyManager.Coin -= 500;
			gunScript.spareAmmo += 30;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void Healing_Buy()
    {
        if (moneyManager.Coin >= 500)
        {
            moneyManager.Coin -= 1000;
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
            if (GunIndex < shop_Main_Gun_Value.Count - 1)
            {
                GunIndex++;
            } else
            {
                GunIndex = 0;
            }

			MainGun.sprite = shop_Main_Gun_Value[GunIndex].GunImage;
			MainGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
		}
        if(isMainGun == false) 
        {
            if (GunIndex < shop_Sub_Gun_Value.Count - 1)
            {
                GunIndex++;
            } else
            {
                GunIndex = 0;
            }

			SubGun.sprite = shop_Sub_Gun_Value[GunIndex].GunImage;
			SubGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
		}
    }

    public void Gun_Change_Minus()
    {
        if (isMainGun == true)
        {
            if (GunIndex > 0)
            {
                GunIndex--;
            } else
            {
                GunIndex = shop_Main_Gun_Value.Count - 1;
            }

			MainGun.sprite = shop_Main_Gun_Value[GunIndex].GunImage;
			MainGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
		}
        if (isMainGun == false)
        {
            if (GunIndex > 0)
            {
                GunIndex--;
            } else
            {
                GunIndex = shop_Sub_Gun_Value.Count;
            }
			SubGun.sprite = shop_Sub_Gun_Value[GunIndex].GunImage;
			SubGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
		}
    }

    public IEnumerator NotEnoughMoney()
    {
        moneyManager.Dontmoney.SetActive(true);
        yield return new WaitForSeconds(1f);
        moneyManager.Dontmoney.SetActive(false);
    }
}
