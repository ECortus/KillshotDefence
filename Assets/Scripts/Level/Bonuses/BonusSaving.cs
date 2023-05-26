using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BonusSaving
{
    public static List<Bonus> UsedBonused = new List<Bonus>();

    public static void AddUsedBonus(Bonus bonus)
    {
        UsedBonused.Add(bonus);
    }

    public static void CancelAllBonuses()
    {
        foreach(Bonus bonus in UsedBonused)
        {   
            bonus.Cancel();
        }
    }
}
