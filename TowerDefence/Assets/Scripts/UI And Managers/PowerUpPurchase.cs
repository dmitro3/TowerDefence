using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PowerUpPurchase : MonoBehaviour
{
    [SerializeField] GameObject[] max_hp_obj;
    [SerializeField] GameObject[] damage_obj;
    [SerializeField] GameObject[] ammo_obj;

    [SerializeField] GameObject hp_buy_btn;
    [SerializeField] GameObject damage_buy_btn;
    [SerializeField] GameObject ammo_buy_btn;

    [SerializeField] TMP_Text hp_text;
    [SerializeField] TMP_Text damage_text;
    [SerializeField] TMP_Text ammo_text;

    public bool resetData = false;

    private void OnEnable()
    {
        
        SetAmountInUI();
    }

    private void SetAmountInUI()
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();

        if (resetData)
        {
            data.upgraded_ammocount = 0;
            data.upgraded_damage = 1f;
            data.upgraded_hp = 0;
            DatabaseManager.Instance.UpdateData(data);
        }

        for (int i = 0; i < max_hp_obj.Length; i++)
        {
            
            if (data.upgraded_hp / 2 > i)
            {
                
                    max_hp_obj[i].GetComponent<Image>().color = Color.green;                    
               
            }
            else
            {
                max_hp_obj[i].GetComponent<Image>().color = Color.white;
            }
        }

        for (int i = 0; i < damage_obj.Length; i++)
        {

            if (data.upgraded_damage / 0.2f > (i + 6) )
            {               
                    
                damage_obj[i].GetComponent<Image>().color = Color.green;
                    
              
            }
            else
            {
                damage_obj[i].GetComponent<Image>().color = Color.white;
            }
        }

        for (int i = 0; i < ammo_obj.Length; i++)
        {

            if (data.upgraded_ammocount / 6 > (i))
            {   
                ammo_obj[i].GetComponent<Image>().color = Color.green;
            }
            else
            {
                ammo_obj[i].GetComponent<Image>().color = Color.white;
            }
        }

        
        ammo_buy_btn.SetActive(data.upgraded_ammocount != 30);
        damage_buy_btn.SetActive(data.upgraded_damage < 2f);
        hp_buy_btn.SetActive(data.upgraded_hp != 10);

        hp_text.text = "+" + data.upgraded_hp.ToString();
        damage_text.text = "x" + data.upgraded_damage.ToString();
        ammo_text.text = "+" + data.upgraded_ammocount.ToString();
    }

    public void BuyMaxHP()
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();
        
        if (data.coins >= 500)
        {
            data.coins -= 500;
            data.upgraded_hp += 2;
            DatabaseManager.Instance.UpdateData(data);
            SetAmountInUI();
        }
        else
        {
            MessaeBox.insta.showMsg("Not Enough Coins", true);
        }

    }
    public void BuyDamage()
    {
        LocalData data = DatabaseManager.Instance.GetLocalData();
        if (data.coins >= 500)
        {
            data.coins -= 500;
            data.upgraded_damage += 0.2f;
            DatabaseManager.Instance.UpdateData(data);
            SetAmountInUI();
        }
        else
        {
            MessaeBox.insta.showMsg("Not Enough Coins", true);
        }
        
    }
    public void BuyAmmo()
    {
        
        LocalData data = DatabaseManager.Instance.GetLocalData();
        if (data.coins >= 500)
        {
            data.coins -= 500;
            data.upgraded_ammocount += 6;
            DatabaseManager.Instance.UpdateData(data);
            SetAmountInUI();
        }
        else
        {
            MessaeBox.insta.showMsg("Not Enough Coins",true);
        }
    }
}
