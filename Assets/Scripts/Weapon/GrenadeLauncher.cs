using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapon
{
    public override string Name => "GrenadeLauncher";
    protected override int DefaultLevel => -1;
    public override ShootingType ShootType => ShootingType.Hold;
    protected override string PrefsKey => Name;
    protected override ObjectType AmmoType => ObjectType.GrenadeAmmo;

    public override void Shot()
    {
        base.Shot();
        
        Vector3 pos = muzzle.position;
        Vector3 rot = muzzle.eulerAngles;

        ObjectPool.Instance.InsertAmmo(AmmoType, ammo, pos, rot, this);
    }
}
