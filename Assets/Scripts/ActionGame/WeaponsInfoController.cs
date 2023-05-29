using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsInfoController : MonoBehaviour
{
    public static WeaponsInfoController Instance { get; set; }
    void Awake() => Instance = this;

    [SerializeField] private List<AvailableToArm> WeaponInfos = new List<AvailableToArm>();

    public string DefaultWeapon
    {
        get
        {
            string def;
            if(GetWeapon("Minigun").Level > -1) 
            {
                def = "Minigun";
                PlayerPrefs.SetString($"{defaultName}{0}", "Minigun");
            }
            else def = "Pistol";

            return def;
        }
    }

    int weaponCount => GameManager.Instance.shooting.Weapons.Count;
    string defaultName = "WeaponInfo";
    public List<string> RequireToArm
    {
        get
        {
            List<string> names = new List<string>{DefaultWeapon};

            string name;
            int count = weaponCount;
            for(int i = 0; i < count; i++)
            {
                name = PlayerPrefs.GetString($"{defaultName}{i}", "");
                /* if(names.Contains(name))
                {
                    count++;
                    continue;
                }
                names.Add(name); */
                if(name != "")
                {
                    if(names.Contains(name))
                    {
                        count++;
                        continue;
                    }
                    names.Add(name);
                }
            }
            return names;
        }
        set
        {
            List<string> names = value;
            List<string> have = new List<string>();

            for(int i = 0; i < weaponCount; i++)
            {
                if(names.Count > 0)
                {
                    if(names[0] == "" || have.Contains(names[0]))
                    {
                        names.Remove(names[0]);
                        i--;
                        continue;
                    }

                    PlayerPrefs.SetString($"{defaultName}{i}", names[0]);

                    have.Add(names[0]);
                    names.Remove(names[0]);
                }
                else
                {
                    PlayerPrefs.SetString($"{defaultName}{i}", "");
                }
            }
            PlayerPrefs.Save();
        }
    }

    /* void Start()
    {
        Refresh();
    } */

    public Weapon GetWeapon(string name)
    {
        foreach(AvailableToArm arm in WeaponInfos)
        {
            if(arm.Info.Weapon.Name == name) return arm.Info.Weapon;
        }

        return null;
    }

    public void UpAllWeapons()
    {
        WeaponInfoUI info = null;

        for(int i = 0; i < WeaponInfos.Count; i++)
        {
            info = WeaponInfos[i].Info;

            if(info.Active && info.Weapon.Level != info.Weapon.MaxLevel)
            {
                info.Weapon.UpLevel();
                info.Weapon.FalseUpgrades++;
            }
        }
    }
    public void DownAllWeapons()
    {
        WeaponInfoUI info = null;

        for(int i = 0; i < WeaponInfos.Count; i++)
        {
            info = WeaponInfos[i].Info;

            if(info.Active && info.Weapon.Level != 0)
            {
                info.Weapon.DownLevel();
                info.Weapon.FalseUpgrades--;
            }
        }
    }

    public void AddCDBonusAllWeapons(int amnt)
    {
        for(int i = 0; i < WeaponInfos.Count; i++)
        {
            WeaponInfos[i].Info.Weapon.SetCDBonus(amnt);
        }
    }
    public void ResetCDBonusAllWeapons()
    {
        for(int i = 0; i < WeaponInfos.Count; i++)
        {
            WeaponInfos[i].Info.Weapon.ResetCDBonus();
        }
    }

    public void AddRequireWeapon(string name)
    {
        if(!RequireToArm.Contains(name))
        {
            List<string> toarm = RequireToArm;
            toarm.Add(name);
            RequireToArm = toarm;
        }
        Refresh();
    }

    public void RemoveRequireWeapon(string name)
    {
        if(RequireToArm.Contains(name))
        {
            List<string> toarm = RequireToArm;
            toarm.Remove(name);
            RequireToArm = toarm;
        }
        Refresh();
    }

    public void ResetOnBiom()
    {
        RequireToArm = new List<string>{DefaultWeapon};
        /* ResetOnLevel(); */
    }

    public void ResetOnLevel()
    {
        for(int i = 0; i < WeaponInfos.Count; i++)
        {
            /* WeaponInfos[i].Info.Weapon.CanBeDown = false; */
            WeaponInfos[i].Info.Reset();
        }

        Refresh();
    }

    void Refresh()
    {
        foreach(AvailableToArm ata in WeaponInfos)
        {
            if(RequireToArm.Contains(ata.Name)) ata.Info.On();
            else ata.Info.Off();
        }
    }
}

[System.Serializable]
public class AvailableToArm
{
    public string Name = "@";
    public WeaponInfoUI Info;
}
