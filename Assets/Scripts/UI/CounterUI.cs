using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CounterUI : MonoBehaviour
{
    protected virtual float CurrentValue { get; }
    protected virtual float MaxValue { get; }

    [Header("Info counter ref-s: ")]
    [SerializeField] private TextMeshProUGUI counter;
    
    public void Refresh()
    {
        counter.text = $"{CurrentValue}/{MaxValue}";
    }
}
