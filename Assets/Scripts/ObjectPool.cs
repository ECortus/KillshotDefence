using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    void Awake() => Instance = this;

    private List<Ammo> BulletPool = new List<Ammo>();
    private List<Ammo> GrenadePool = new List<Ammo>();
    private List<Ammo> RocketPool = new List<Ammo>();
    private List<Ammo> BuckshotPool = new List<Ammo>();

    public GameObject InsertAmmo(ObjectType type, GameObject obj, Vector3 pos, Vector3 rot, Weapon weapon = null)
    {
        List<Ammo> list = new List<Ammo>();

        switch(type)
        {
            case ObjectType.BulletAmmo:
                list = BulletPool;
                break;
            case ObjectType.RocketAmmo:
                list = RocketPool;
                break;
            case ObjectType.GrenadeAmmo:
                list = GrenadePool;
                break;
            case ObjectType.ShotgunAmmo:
                list = BuckshotPool;
                break;
            default:
                return null;
        }

        foreach(Ammo am in list)
        {
            if(am == null) continue;

            if(!am.Active) 
            {
                am.Reset(pos, rot);
                
                if(weapon != null)
                {
                    am.SetDamage(weapon.Damage);
                }

                am.On();
                return am.gameObject;
            }
        }

        Ammo scr = Instantiate(obj, pos, Quaternion.Euler(rot)).GetComponent<Ammo>();

        /* scr.Reset(pos, rot); */

        if(weapon != null)
        {
            scr.SetDamage(weapon.Damage);
        }

        /* scr.On(); */

        list.Add(scr);

        switch(type)
        {
            case ObjectType.BulletAmmo:
                BulletPool = list;
                break;
            case ObjectType.RocketAmmo:
                RocketPool = list;
                break;
            case ObjectType.GrenadeAmmo:
                GrenadePool = list;
                break;
            case ObjectType.ShotgunAmmo:
                BuckshotPool = list;
                break;
            default:
                return null;
        }

        return scr.gameObject;
    }
}

[System.Serializable]
public enum ObjectType
{
    Default, BulletAmmo, GrenadeAmmo, ShotgunAmmo, RocketAmmo
}
