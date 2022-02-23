using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    [Header("Init Hex-Grid")]
    [SerializeField] Transform circlePrefab;
    [SerializeField] float gap;
    [SerializeField] int battleRadius = 11;
    [SerializeField] Transform gridParent;

    [Header("Team Attack")]
    [SerializeField] BattleTeam attackTeam;
    public BattleTeam AttackTeam => attackTeam;

    [Header("Team Defense")]
    [SerializeField] BattleTeam defenseTeam;
    public BattleTeam DefenseTeam => defenseTeam;

    [HideInInspector]
    public List<HexCircle> circlesPool;

    int gridWidth, gridHeight;
    int seatsOnRow;
    Vector2 startPos;
    Vector2 circleSize;

    Queue<Character> actorsQueue;

    // Start is called before the first frame update
    void Start()
    {
        actorsQueue = new Queue<Character>();
        circlesPool = new List<HexCircle>();
        gridWidth = gridHeight = battleRadius;
        seatsOnRow = gridHeight / 2 + 1;
        AddGap();
        CalcStartPos();
        GenerateBattleArea();
        InitCharacter();

        GameManager.Instance.battleArea = this;
    }

    private void OnEnable()
    {
        CallBackService.OnAddActor += OnAddActor;

    }

    private void OnDisable()
    {
        CallBackService.OnAddActor -= OnAddActor;
    }

    void OnAddActor(Character actor)
    {
        actorsQueue.Enqueue(actor);
    }

    private void FixedUpdate()
    {
        if (actorsQueue.Count > 0 && GM.IsPauseGame == false)
        {
            var pickup = actorsQueue.Count > GM.Define.actionCountInFrame ? 
                GM.Define.actionCountInFrame : actorsQueue.Count;
            for (int i = 0; i < pickup; i++)
            {
                var go = actorsQueue.Dequeue();
                go.Action();
            }
        }
    }

    private void InitCharacter()
    {
        int teamRadius = GM.Define.teamSizeNormal;

        int ignoreRadius = teamRadius + 1;

        var middleIndex = circlesPool.Count / 2;
        Debug.Log(circlesPool.Count + " ---- " + middleIndex);
        Point middlePoint = circlesPool[middleIndex].PointIndex;

        defenseTeam.Init(this, middlePoint, teamRadius);

        attackTeam.Init(this, middlePoint, teamRadius , ignoreRadius);
    }

    private void NewCircle(int x, int y, Vector2 pos)
    {
        var go = Instantiate<Transform>(circlePrefab, gridParent);
        var circle = go.GetComponent<HexCircle>();
        circle.transform.position = pos;
        circle.name = x + " | " + y;
        circle.PointIndex = new Point(x, y);
        circlesPool.Add(circle);
    }

    public void GenerateBattleArea()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (CalcPosition(x, y, out Vector2 pos))
                {
                    NewCircle(x, y, pos);

                    if (y == gridHeight / 2)
                    {
                        if (x == gridWidth - 1)
                        {
                            CalcPosition(x + 1, y, out pos);
                            NewCircle(x + 1, y, pos);
                        }
                        else if (x == 0)
                        {
                            circlesPool[circlesPool.Count - 1].gameObject.SetActive(false);
                        }
                    }
                }
            }
            if (y < gridHeight / 2)
            {
                seatsOnRow++;
            }
            else if (y >= gridHeight / 2)
            {
                seatsOnRow--;
            }
        }
    }

    bool CalcPosition(int x, int y, out Vector2 pos, bool forcePos = false)
    {
        pos = Vector2.negativeInfinity;

        var ignoreL = (gridWidth - seatsOnRow) / 2;
        var ignoreR = seatsOnRow + ignoreL + 1;
        if ((x <= ignoreL || x >= ignoreR) && gridWidth - seatsOnRow != 0 && !forcePos)
        {
            return false;
        }

        float offset = 0;
        if (y % 2 != 0)
        {
            offset = -circleSize.x / 2;
        }

        float tmpX = startPos.x + x * circleSize.x + offset;
        float tmpY = startPos.y - y * circleSize.y * 0.75f;

        pos = new Vector2(tmpX, tmpY);
        return true;
    }

    void AddGap()
    {
        circleSize = circlePrefab.localScale * gap;
    }

    void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = circleSize.x / 2;

        float x = -circleSize.x * (gridWidth / 2) - offset;
        float z = circleSize.y * 0.75f * (gridHeight / 2);

        startPos = new Vector2(x, z);
    }

    public void RefreshCircle()
    {
        var insides = circlesPool.Where(hex => hex.IsInsideCameraView).ToList();

        foreach (var item in insides)
        {
            item.GetComponent<SpriteRenderer>().color = Color.white;
            item.SetBlur(true);
        }
    }

    public List<HexCircle> GetNeighbor(Point p, int takeRadius = 1)
    {
        return GetNeighbor(p.x, p.y, takeRadius);
    }

    private List<HexCircle> GetNeighbor(int x, int y, int takeRadius = 1)
    {
        var diameter = takeRadius * 2 + 1;

        var startX = x - takeRadius; // move to the left of this row

        var tempList = new List<HexCircle>();
        int coutOffet = 0;
        for (int t = 0; t < takeRadius + 1; t++)
        {
            if (t == 0)// get row at middle first
            {
                PickupCircleInRow(startX, y, diameter, ref tempList);
            }
            else
            {
                coutOffet++;
                if (coutOffet == 3) // calculate startX every 2 rows
                {
                    if (y % 2 != 0)
                        startX -= 1;
                    else
                        startX += 1;
                    coutOffet = 1;
                }

                int offset = 0;
                if (y % 2 == 0) // add 1 when picking row is even number
                    offset = 1;

                PickupCircleInRow(startX + offset, y + t, diameter, ref tempList);
                PickupCircleInRow(startX + offset, y - t, diameter, ref tempList);

                if (y % 2 == 1)// add 1 when picking row is odd number
                    startX++;
            }
            diameter--;
        }

        return tempList;
    }

    private void PickupCircleInRow(int startX, int y, int diameter, ref List<HexCircle> tempList)
    {
        for (int i = 0; i < diameter; i++)
        {
            var pick = circlesPool.Find(obj => obj.PointIndex.x == startX + i && obj.PointIndex.y == y);
            if (pick != null)
                tempList.Add(pick);
        }
    }

    public List<Character> GetDefendersAround(Point p, int radius)
    {
        var enemies = GetNeighbor(p, radius).Select(hex => hex.Owner).Where(x => x != null && x.Team == Team.Defense).ToList();
        return enemies;
    }

    public void CheckGameOver()
    {
        var def = defenseTeam.TeamMember.Where(x => x.IsAlive()).ToList();
        var atk = attackTeam.TeamMember.Where(x => x.IsAlive()).ToList();
        if (def.Count == 0 && atk.Count == 0)
        {
            Debug.Log("peace");
        }
        else if (def.Count == 0)
        {
            CallBackService.OnEndGame?.Invoke(Team.Attack);
            Victory(atk);
        }
        else if(atk.Count == 0)
        {
            CallBackService.OnEndGame?.Invoke(Team.Defense);
            Victory(def);
        }
    }

    public void Victory(List<Character> winners)
    {
        foreach (var item in winners)
        {
            item.VictoryPose();
        }
    }
}
