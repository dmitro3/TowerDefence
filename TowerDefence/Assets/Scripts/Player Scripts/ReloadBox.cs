using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadBox : MonoBehaviour
{
    [SerializeField] Sprite ClosedBox;
    [SerializeField] float RespawnTime = 30;
    //[SerializeField] Sprite OpenBox;
    [SerializeField] List<Transform> SpawnPositions = new List<Transform>();
    
    
    private Type boxType;
    private SpriteRenderer _renderer;
    private Transform tempTransform;
    private HeadQuarters HQ;

    public static int chanceForAmmo = 85;
    public static int chanceForRepair = 5;

    private void OnEnable()
    {
        _renderer.sprite = ClosedBox;
        RelocateToNewPos();
        ChooseBoxType();
    }

    private void ChooseBoxType()
    {
        int type = UnityEngine.Random.Range(0, 100);
        if (type >= 0 && type < chanceForAmmo)
        {
            boxType = Type.Ammo;
            _renderer.color = ammo_color;
        }
        else if (type >= chanceForAmmo && type < 100-chanceForRepair)
        {
            boxType = Type.HP;
            _renderer.color = health_color;
            chanceForAmmo += 5;
        }
        else
        {
            chanceForAmmo += 5;
            chanceForRepair = 0;
            boxType = Type.Repair;
            _renderer.color = repair_color;
        }

        Debug.Log("chance for ammo is " + chanceForAmmo);
        Debug.Log("chance for Repair is " + chanceForRepair);
    }

    private void RelocateToNewPos()
    {
        List<Transform> availablePos = new List<Transform>();
        foreach (var item in SpawnPositions)
        {
            if (item.gameObject.activeSelf) availablePos.Add(item);
        }

        int PosID = UnityEngine.Random.Range(0, availablePos.Count);
        tempTransform = availablePos[PosID];
        transform.position = tempTransform.position;
        tempTransform.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        tempTransform.gameObject.SetActive(true);
        Invoke(nameof(RefillBox), RespawnTime);
    }


    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        HQ = FindObjectOfType<HeadQuarters>();

    }

    [SerializeField] Color ammo_color;
    [SerializeField] Color health_color;
    [SerializeField] Color repair_color;
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.TryGetComponent<PlayerController>(out PlayerController PC);
        if(PC != null)
        {
            switch(boxType)
            {
                case Type.Ammo:
                    {
                        if (PC.Ammo >= PC.MaxAmmo) return;

                        PC.Ammo = PC.MaxAmmo;
                        MessaeBox.insta.ShowInformationMsg(ammo_color, "Ammo Reloaded");
                        //_renderer.sprite = OpenBox;
                        gameObject.SetActive(false);
                    }
                    break;
                case Type.HP:
                    {
                        if (PC.Health >= PC.MaxHealth) return;
                        chanceForAmmo -= 5;
                        PC.Health ++;
                        PC.Health = Mathf.Clamp(PC.Health, 1, PC.MaxHealth);
                        MessaeBox.insta.ShowInformationMsg(health_color, "Health Gained");
                        //_renderer.sprite = OpenBox;
                        gameObject.SetActive(false);
                    }
                    break;
                case Type.Repair:
                    {
                        if (HQ.Health >= HQ.MaxHealth) return;
                        chanceForAmmo -= 5;
                        chanceForRepair = 5;
                        HQ.Health++;
                        HQ.Health = Mathf.Clamp(HQ.Health, 1, HQ.MaxHealth);
                        MessaeBox.insta.ShowInformationMsg(repair_color, "HQ Repaired");
                        // _renderer.sprite = OpenBox;
                        gameObject.SetActive(false);
                    }
                    break;
            }
            
        }
    }

    private void RefillBox()
    {
        this.gameObject.SetActive(true);
    }
    private enum Type
    {
        Ammo,
        HP,
        Repair
    }
}


