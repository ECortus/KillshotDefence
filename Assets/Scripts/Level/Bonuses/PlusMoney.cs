using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "-", menuName = "MoneyBonus")]
public class PlusMoney : Bonus
{
    public override BonusType Type => BonusType.PlusMoney;

    public int Amount = 500;

    public override void Apply()
    {
        Money.Plus(Amount);
    }

    public override void Cancel()
    {

    }
}
