using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsInfoController : MonoBehaviour
{
    [SerializeField] private List<AvailableToArm> WeaponInfos = new List<AvailableToArm>();
    private List<string> RequireToArm = new List<string>{"Minigun"};

    /* void Start()
    {
        Refresh();
    } */

    public void AddRequireWeapon(string name)
    {
        if(!RequireToArm.Contains(name))
        {
            RequireToArm.Add(name);
        }
        Refresh();
    }

    public void RemoveRequireWeapon(string name)
    {
        if(RequireToArm.Contains(name))
        {
            RequireToArm.Remove(name);
        }
        Refresh();
    }

    public void Reset()
    {
        RequireToArm = new List<string>{"Minigun", "GrenadeLauncher", "Shotgun", "RocketLauncher"};
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
