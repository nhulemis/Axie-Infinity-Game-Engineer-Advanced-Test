using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance => instance;

    public BattleArea battleArea;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public static class GM
{
    public static BattleArea BattleArea
    {
        get => GameManager.Instance.battleArea;
        set => GameManager.Instance.battleArea = value;
    }
}
