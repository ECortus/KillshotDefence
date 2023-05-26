using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "-", menuName = "UpWeaponsLevelOnThisLevel")]
public class UpWeaponsLevelOnThisLevel : Bonus
{
    public override BonusType Type => BonusType.UpWeaponsLevelOnThisLevel;

    public override void Apply()
    {
        WeaponsInfoController.Instance.UpAllWeapons();
    }

    public override void Cancel()
    {
        WeaponsInfoController.Instance.DownAllWeapons();
    }
}
