using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoSliderUI : MonoBehaviour
{
    protected virtual float CurrentValue { get; }
    protected virtual float MaxValue { get; }

    [Header("Info slider ref-s: ")]
    [SerializeField] private Slider slider;

    int sign
    {
        get
        {
            return CurrentValue > slider.value ? 1 : -1;
        }
    }

    Coroutine coroutine;
    
    public void Refresh()
    {
        slider.interactable = false;

        slider.minValue = 0f;
        slider.maxValue = MaxValue;

        /* slider.value = CurrentValue; */
        if(coroutine == null) coroutine = StartCoroutine(Smooth());
    }

    IEnumerator Smooth()
    {
        while(slider.value != CurrentValue)
        {
            slider.value += Time.deltaTime * (MaxValue / 0.9f) * sign;

            if(Mathf.Abs(slider.value - CurrentValue) < 1) break;

            yield return null;
        }

        slider.value = CurrentValue;

        /* StopCoroutine(coroutine); */
        coroutine = null;
    }
}
