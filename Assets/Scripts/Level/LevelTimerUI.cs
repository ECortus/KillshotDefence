using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelTimerUI : MonoBehaviour
{
    [SerializeField] private LevelTimer timer;
    [SerializeField] private TextMeshProUGUI text;

    public void Refresh()
    {
        text.text = $"{(int)(timer.TimeToWin - timer.time)}";
    }
}
