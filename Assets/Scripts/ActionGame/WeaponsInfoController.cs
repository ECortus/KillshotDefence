using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsInfoController : MonoBehaviour
{
    public static WeaponsInfoController Instance { get; set; }
    void Awake() => Instance = this;

    [SerializeField] private List<AvailableToArm> WeaponInfos = new List<AvailableToArm>();

    public string DefaultWeapon => "Pistol";

    public int WeaponCount => 4;
    string defaultName = "WeaponInfo";

    public List<string> RequireToArm = new List<string>{};
    
    public void Save()
    {
        List<string> names = RequireToArm;

        for(int i = 0; i < names.Count; i++)
        {
            PlayerPrefs.SetString($"{defaultName}{i}", names[i]);
        }

        PlayerPrefs.Save();
    }

    public void Load()
    {
        List<string> names = new List<string>();

        if(PlayerPrefs.GetString($"{defaultName}{0}", "") == "") names.Add(DefaultWeapon);

        string name;
        int count = WeaponCount;
        
        for(int i = 0; i < count; i++)
        {
            name = PlayerPrefs.GetString($"{defaultName}{i}", "");
            if(name != "")
            {
                names.Add(name);
            }
        }

        List<string> filtered = new List<string>();
        foreach(string nam in names)
        {
            if(!filtered.Contains(nam))
            {
                filtered.Add(nam);
            }
        }

        RequireToArm = filtered;
    }

    void Start()
    {
        Load();
    }

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

                info.Reset();
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

            /* for(int i = 0; i < toarm.Count; i++)
            {
                if(toarm[i] == "")
                {
                    toarm.Remove(toarm[i]);
                    i--;
                }
            } */

            if(toarm.Count == WeaponCount)
            {
                if(toarm[toarm.Count - 1] != "") toarm.RemoveAt(0);
            }

            toarm.Add(name);
            /* if(toarm.Count < 4) toarm.Add(name);
            else toarm[3] = name; */

            RequireToArm = toarm;
        }

        Save();
        Refresh();
    }

    public void RemoveRequireWeapon(string name)
    {
        if(RequireToArm.Contains(name))
        {
            List<string> toarm = RequireToArm;
            toarm.Remove(name);
            /* toarm[toarm.IndexOf(name)] = ""; */
            RequireToArm = toarm;
        }

        Save();
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
