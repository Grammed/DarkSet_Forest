using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop_Manager : MonoBehaviour
{
    [Header("���� �Ŵ���")]
    WeaponManager weaponManager;

    [Header("��")]
    public Gun gunScript;
    [Header("Gun_info"),SerializeField]
    private List<SO_Gun> shop_Main_Gun_Value;
    [SerializeField]
    private List<SO_Gun> shop_Sub_Gun_Value;

    [Header("�ѱ� ����")]
    public Image MainGun;
    public Image SubGun;
    public Text MainGuntext;
    public Text SubGuntext;
    public Text gunValueCost;

    [Header("�ѱ� Panel")]
    public GameObject MainGun_Panel;
    public GameObject SubGun_Panel;

    [Header("�μ�")]
    public Text Bullettext;
    public int Bullet;
    public Text HPtext;
    public PlayerController player;

    private int GunIndex;

    private bool isMainGun;

    [SerializeField]
    private MoneyManager moneyManager;


	private void Start()
	{
		weaponManager = FindObjectOfType<WeaponManager>();
		moneyManager = FindObjectOfType<MoneyManager>();

        moneyManager.Coin = 1000;
        player = FindObjectOfType<PlayerController>();
	}

	void OnEnable() // OnEnable�� �ٲ���� SetAcitve�ҷ���
    {
        MainGun.sprite = shop_Main_Gun_Value[0].GunImage;
        SubGun.sprite = shop_Sub_Gun_Value[0].GunImage;
        isMainGun = true;
        MainGun_Panel.SetActive(true);
        SubGun_Panel.SetActive(false);

        GunIndex = 0;
		MainGun.sprite = shop_Main_Gun_Value[GunIndex].GunImage;
		MainGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
		Bullettext.text = Bullet.ToString();
        //HPtext.text = HP.ToString();
        moneyManager.Dontmoney.SetActive(false);
        gunValueCost.text = shop_Main_Gun_Value[GunIndex].Cost + " Coin";


        // Bullet = gunScript.spareAmmo; //����
        // moneyManager.Coin = 1000000; // ����
    }

    private void Update()
    {
       var usedGun = GameObject.FindObjectOfType<Gun>();
        moneyManager.Coin_Text.text = moneyManager.Coin.ToString();
        Bullettext.text = "�Ѿ� ���� : " + usedGun.spareAmmo.ToString() + "��";
        HPtext.text = "���� ü�� : " + (int)(player.HP.value * 100);
    }

    public void Main_Weapon_Select()
    {
        isMainGun = true;
        MainGun_Panel.SetActive(true);
        SubGun_Panel.SetActive(false);
        GunIndex = 0;
        MainGun.sprite = shop_Main_Gun_Value[GunIndex].GunImage;
        MainGuntext.text = shop_Main_Gun_Value[GunIndex].GunName;
        gunValueCost.text = shop_Main_Gun_Value[GunIndex].Cost + " Coin";
        print(shop_Main_Gun_Value[GunIndex].Cost);
    }

    public void Sub_Weapon_Select()
    {
        isMainGun = false;
        MainGun_Panel.SetActive(false);
        SubGun_Panel.SetActive(true);
        GunIndex = 0;

        SubGun.sprite = shop_Sub_Gun_Value[GunIndex].GunImage;
        SubGuntext.text = shop_Sub_Gun_Value[GunIndex].GunName;
        gunValueCost.text = shop_Sub_Gun_Value[GunIndex].Cost.ToString() + " Coin";
    }

    public void MainGun_Buy()
    {
        if (moneyManager.Coin >= shop_Main_Gun_Value[GunIndex].Cost && !weaponManager.primaryWeapon || weaponManager.primaryWeapon.name != shop_Main_Gun_Value[GunIndex].gunName)
        {
            moneyManager.Coin -= shop_Main_Gun_Value[GunIndex].Cost;
            weaponManager.ChangePrimary(shop_Main_Gun_Value[GunIndex].prefab, shop_Main_Gun_Value[GunIndex]);
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void SubGun_Buy()
    {
        if (moneyManager.Coin >= shop_Sub_Gun_Value[GunIndex].Cost && !weaponManager.secondaryWeapon || weaponManager.secondaryWeapon.name != shop_Sub_Gun_Value[GunIndex].gunName)
        {
            moneyManager.Coin -= shop_Sub_Gun_Value[GunIndex].Cost;
            weaponManager.ChangeSecondary(shop_Sub_Gun_Value[GunIndex].prefab, shop_Sub_Gun_Value[GunIndex]);
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
    }

    public void Bullet_Buy()
    {
        //SO_Gun so_gun = weaponManager.primaryWeapon.GetComponent<SO_Gun>();
        var usedGun = GameObject.FindObjectOfType<Gun>();
        if (moneyManager.Coin >= 500 && usedGun.maxSpareAmmo > usedGun.spareAmmo)
        {
            
            moneyManager.Coin -= 500;
            usedGun.spareAmmo = usedGun.maxSpareAmmo;
        }
        else
        {
            StartCoroutine("NotEnoughMoney");
        }
        //usedGun.spareAmmo += 60;
        //Bullettext.text = "�Ѿ� ���� : " + usedGun.spareAmmo.ToString() + "��";
        //so_gun.spareAmmo += 60;
        //weaponManager.ChangePrimary(weaponManager.primaryWeapon, so_gun);
    }

    public void Healing_Buy()
    {
        if (moneyManager.Coin >= 1000 && player.HP.value != 1f)
        {
            moneyManager.Coin -= 1000;
            Debug.Log("ü��ȸ��");

            player.HP.value = Mathf.Min(player.HP.value + 0.5f, 1f);
            
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
            gunValueCost.text = shop_Main_Gun_Value[GunIndex].Cost + " Coin";

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
			SubGuntext.text = shop_Sub_Gun_Value[GunIndex].GunName;
            gunValueCost.text = shop_Sub_Gun_Value[GunIndex].Cost + " Coin";

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
            gunValueCost.text = shop_Main_Gun_Value[GunIndex].Cost + " Coin";
        }
        if (isMainGun == false)
        {
            if (GunIndex > 0)
            {
                GunIndex--;
            } else
            {
                GunIndex = shop_Sub_Gun_Value.Count - 1;
            }
			SubGun.sprite = shop_Sub_Gun_Value[GunIndex].GunImage;
			SubGuntext.text = shop_Sub_Gun_Value[GunIndex].GunName;
            gunValueCost.text = shop_Sub_Gun_Value[GunIndex].Cost + " Coin";
        }
    }

    public IEnumerator NotEnoughMoney()
    {
        moneyManager.Dontmoney.SetActive(true);
        yield return new WaitForSeconds(1f);
        moneyManager.Dontmoney.SetActive(false);
    }

    public void Escape(int requiredMoney)
    {
        if (moneyManager.Coin >= requiredMoney)
        {
            GameManager.Instance.WinGame();
        } else
        {
            StartCoroutine(nameof(NotEnoughMoney));
        }
    }
}
