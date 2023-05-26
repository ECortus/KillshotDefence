using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContinueLevelButton : MonoBehaviour
{
    private int index => Statistics.LevelIndex;
    [SerializeField] private TextMeshProUGUI lvltext;

    void Update()
    {
        /* lvltext.text = $"Level {index + 1}"; */
        lvltext.text = $"Level {LevelManager.Instance.GetIndex() + 1}";
    }
}
