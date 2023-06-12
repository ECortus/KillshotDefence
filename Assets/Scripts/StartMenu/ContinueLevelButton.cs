using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContinueLevelButton : MonoBehaviour
{
    private int index => Statistics.LevelIndex;
    [SerializeField] private TextMeshProUGUI lvlText, counterText;

    void Update()
    {
        /* lvltext.text = $"Level {index + 1}"; */
        lvlText.text = $"Level {LevelManager.Instance.GetIndex() + 1}";
        counterText.text = $"Weapons {WeaponsInfoController.Instance.RequireToArm.Count}/{WeaponsInfoController.Instance.WeaponCount}";
    }
}
