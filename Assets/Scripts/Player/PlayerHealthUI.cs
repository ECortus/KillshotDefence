using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : /* InfoSliderUI */ MonoBehaviour
{
    [Space]
    [SerializeField] private PlayerHealth ph;
    [SerializeField] private TextMeshProUGUI text;

    /* protected override  */float CurrentValue => ph.Health;
    /* protected override  */float MaxValue => ph.MaxHealth;

    public void Refresh()
    {
        /* text.text = $"{(int)((CurrentValue/MaxValue) * 100)}%"; */
        text.text = $"{(int)(CurrentValue)}%";
    }
}
