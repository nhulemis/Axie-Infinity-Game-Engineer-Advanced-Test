using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance => instance;

    [HideInInspector] public BattleArea battleArea;

    [SerializeField] TextMeshProUGUI fps;
    [SerializeField] public GameDefine gameDefine;

    float updateFPS = 2;
    float time = 0;
    int frame;

    [HideInInspector]  public int avgFrameRate;
    [HideInInspector]  public bool isEndGame;
    [HideInInspector]  public bool isPauseGame;

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
        isEndGame = false;
        isPauseGame = false;
    }
    private void OnEnable()
    {
        CallBackService.OnEndGame += OnEndGame;
    }
    private void OnDisable()
    {
        CallBackService.OnEndGame -= OnEndGame;
    }

    void OnEndGame(Team winner)
    {
        Debug.Log(winner.ToString() + " wwin");
        isEndGame = true;
    }

    // Update is called once per frame
    void Update()
    {
        float current = Time.frameCount / Time.time;
        avgFrameRate = (int)current;
        fps.text = avgFrameRate.ToString() + " FPS";
    }
}

public static class GM
{
    public static BattleArea BattleArea => GameManager.Instance.battleArea;
    public static bool IsEndGame => GameManager.Instance.isEndGame;
    public static bool IsPauseGame => GameManager.Instance.isPauseGame;

    public static GameDefine Define => GameManager.Instance.gameDefine;
}
