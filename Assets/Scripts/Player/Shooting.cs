using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Shooting : MonoBehaviour
{
    [HideInInspector] public bool IsShooting = false;
    private bool SomethingArmed = false;

    [SerializeField] private List<ArmedWeapon> Weapons = new List<ArmedWeapon>();

    private int Index = 0;
    public void SetWeapon(string name)
    {
        ArmedWeapon armed;
        bool have = false;

        for(int i = 0; i < Weapons.Count; i++)
        {
            armed = Weapons[i];
            if(armed.Name == name) 
            {
                Index = i;
                armed.Weapon.On();
                have = true;
                SomethingArmed = true;
            }
            else armed.Weapon.Off();
        }

        if(!have) Index = 0;
    }
    public void OffWeapons()
    {
        for(int i = 0; i < Weapons.Count; i++)
        {
            Weapons[i].Weapon.Off();
        }
    }

    private Weapon weapon => Weapons[Index].Weapon;

    private int ReloadTime => (int)(weapon.ReloadTime * 1000);

    Coroutine coroutine;

    /* void Start()
    {
        SetWeapon("Minigun");
    } */

    public void Reset()
    {
        Index = 0;
        SetWeapon("Minigun");
    }

    void Update()
    {
        if(SomethingArmed && !PointerIsOverUI.Instance.CheckThis())
        {
            if(Input.GetMouseButtonDown(0))
            {
                Begin();
            }
        }
    }

    public void FullAmmoAllWeapons()
    {
        foreach(ArmedWeapon wp in Weapons)
        {
            wp.Weapon.FullAmmo();
        }
    }

    public async void Begin()
    {
        await Working();
    }

    async UniTask Working()
    {
        IsShooting = true;

        while(true)
        {
            if(weapon.isReloading)
            {
                await UniTask.WaitUntil(() => !weapon.isReloading);
            }
            
            if(!GameManager.Instance.isActive || !Input.GetMouseButton(0)) break;

            if(weapon.ShootType == ShootingType.Hold)
            {
                await Shoot();
            }
            else if(weapon.ShootType == ShootingType.Tap)
            {
                await UniTask.WaitUntil(() => Input.GetMouseButtonUp(0));
                await Shoot();

                break;
            }
        }

        IsShooting = false;
    }

    public async UniTask Shoot()
    {
        weapon.Shot();
        await weapon.ReduceAmmo();
    }
}

[System.Serializable]
public class ArmedWeapon
{
    public string Name = "@";
    public Weapon Weapon;
}

[System.Serializable]
public enum ShootingType
{
    Tap, Hold
}
