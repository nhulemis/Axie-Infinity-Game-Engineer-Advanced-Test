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
    [SerializeField] public UIController uIController;
    [SerializeField] public CameraController cameraController;
    [HideInInspector] public bool isEndGame;
    [HideInInspector] public bool isPauseGame;
    [HideInInspector] public int currentSpeed;

    float updateFPS = 2;
    float time = 0;
    int frame;
    int avgFrameRate;
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
        isEndGame = true;
        isPauseGame = false;
        currentSpeed = 1;
    }
    private void OnEnable()
    {
        CallBackService.OnEndGame += OnEndGame;
        CallBackService.OnPauseGame += OnPauseGame;
        CallBackService.OnResume += OnResume;
        CallBackService.OnSpeedUp += OnSpeedUp;
        CallBackService.OnGiveUp += OnGiveUp;

    }
    private void OnDisable()
    {
        CallBackService.OnEndGame -= OnEndGame;
        CallBackService.OnPauseGame -= OnPauseGame;
        CallBackService.OnResume -= OnResume;
        CallBackService.OnSpeedUp -= OnSpeedUp;
        CallBackService.OnGiveUp -= OnGiveUp;
    }

    void OnPauseGame()
    {
        isPauseGame = true;
    }

    void OnResume()
    {
        isPauseGame = false;
    }

    void OnSpeedUp()
    {
        currentSpeed = currentSpeed == 1 ? 2 : 1;

        Time.timeScale = currentSpeed;

        if (currentSpeed == 2)
        {
            GM.CamCtrl.PlaySpeedFx(true);
        }
        else
        {
            GM.CamCtrl.PlaySpeedFx(false);
        }
    }

    void OnGiveUp()
    {
        isEndGame = true;
    }

    void OnEndGame(Team winner)
    {
        if (isEndGame)
        {
            return;
        }

        Debug.Log(winner.ToString() + " wwin");
        isEndGame = true;
        var endGamePopup = GM.UI.OpenPopup<PopupEndGame>();
        endGamePopup.FillData(winner);
    }

    public void StartGame(int teamSize)
    {
        var middlePos = battleArea.InitCharacter(teamSize);
        StartCoroutine(StartGameAfter(GM.Define.StartCoolDownTime));
        CallBackService.OnTeamMemberChanged?.Invoke();
        CallBackService.OnStartGame?.Invoke(middlePos);
    }

    IEnumerator StartGameAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        isEndGame = false;
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
    public static UIController UI => GameManager.Instance.uIController;
    public static CameraController CamCtrl => GameManager.Instance.cameraController;

    public static int CurrentGameSpeed => GameManager.Instance.currentSpeed;

    public static void StartGame(int teamsize) => GameManager.Instance.StartGame(teamsize);
}
