using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;

public class UI : MonoBehaviour
{
    public static UI Instance { get; set; }

    [SerializeField] private EndGameUI end, lose;

    void Awake()
    {
        Instance = this;
    }

    public void On()
    {

    }

    public void Off()
    {

    }
    public void Restart()
    {
        lose.Close();
    }

    public void NextLevel()
    {
        end.Close();
    }

    public void EndLevel()
    {
        end.Open();
    }

    public void LoseLevel()
    {
        lose.Open();
    }
}