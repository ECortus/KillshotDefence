using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "-", menuName = "PlusMaxHealthOnThisLevel")]
public class PlusMaxHealthOnThisLevel : Bonus
{
    public override BonusType Type => BonusType.PlusMaxHealthOnThisLevel;

    public float Amount = 50;

    public override void Apply()
    {
        GameManager.Instance.health.AddBonus(Amount);
    }

    public override void Cancel()
    {
        GameManager.Instance.health.ResetBonus();
    }
}
