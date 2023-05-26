using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{
    [SerializeField] private Level level;
    public float TimeToWin => level.LevelWaves.TimeToWin;
    public float time { get; set; }

    [SerializeField] private LevelTimerUI ui;

    public void On()
    {
        this.enabled = true;
        gameObject.SetActive(true);
        time = 0f;
    }

    public void Off()
    {
        this.enabled = false;
        /* gameObject.SetActive(false); */
    }

    void Update()
    {
        time += Time.deltaTime;
        ui.Refresh();

        if(time >= TimeToWin)
        {
            LevelManager.Instance.ActualLevel.EndLevel();
            Off();
        }
    }
}