using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    private bool _GameActive = false;
    public void SetActive(bool value) => _GameActive = value;
    public bool isActive => _GameActive;

    void Awake() => Instance = this;

    public Player player;
    public PlayerHealth health;
    public Shooting shooting;
    public WeaponsInfoController weaponsInfo;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void SetFollowTarget(Transform tf)
    {
        
    }
}
