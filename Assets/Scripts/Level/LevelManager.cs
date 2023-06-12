using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; set; }

    public List<Level> Levels = new List<Level>();

    private int _Index { get { return Statistics.LevelIndex; } set { Statistics.LevelIndex = value; } }
    public int GetRealIndex() => _Index;
    public int GetIndex() => _Index % Levels.Count;
    public void SetIndex(int value) => _Index = value;

    public Level ActualLevel => Levels[GetIndex()];

    [SerializeField] private ChangeGameState changeGameState;

    [Space]
    [SerializeField] private Transform bufferForLevel;

    void Awake() => Instance = this;

    /* void Start()
    {
        LoadOnStart();
    }

    void LoadOnStart()
    {
        LoadLevel();
    } */

    public void LoadLevel()
    {
        /* BufferingLevel(); */
        Level level = ActualLevel;
        level.On();

        ActualLevel.StartLevel();
    }

    public void AddOneToIndex()
    {
        int index = _Index;
        index += 1;
        SetIndex(index);
    }

    public void NextLevel()
    {
        int index = _Index;
        index -= 1;
        SetIndex(index);

        ActualLevel.NextLevel();
        OffLevel(ActualLevel);

        /* GameObject levelPref = GetBufferLevel(); 
        GameObject go = Instantiate(levelPref, transform);
        Level level = go.GetComponent<Level>();

        Levels[GetIndex()] = level;

        OffLevel(level); */

        AddOneToIndex();

        changeGameState.ChangeToStartMenu();
        /* LoadLevel(); */
    }

    public void PreviousLevel()
    {
        OffLevel(ActualLevel);

        int index = _Index;
        index -= 1;
        SetIndex(index);

        LoadLevel();
    }

    public void RestartLevel()
    {
        /* OffLevel(ActualLevel);

        GameObject levelPref = GetBufferLevel(); 
        GameObject go = Instantiate(levelPref, transform);
        Level level = go.GetComponent<Level>();

        Levels[GetIndex()] = level; */
        /* UI.Instance.Restart();
        LoadLevel(); */

        ActualLevel.NextLevel();
        OffLevel(ActualLevel);

        UI.Instance.Restart();
        changeGameState.ChangeToStartMenu();
    }

    public void LoseLevel()
    {
        ActualLevel.LoseLevel();
    }

    void OffLevel(Level level)
    {
        level.Off();
        /* level.Eliminate(); */
    }

    GameObject GetBufferLevel()
    {
        return bufferForLevel.GetChild(0).gameObject;
    }

    void BufferingLevel()
    {
        if(bufferForLevel.childCount > 0) 
        {
            Destroy(GetBufferLevel());
        }

        Level level = ActualLevel;
        GameObject go = Instantiate(level.gameObject, bufferForLevel);

        level = go.GetComponent<Level>();
        level.Off();
    }
}
