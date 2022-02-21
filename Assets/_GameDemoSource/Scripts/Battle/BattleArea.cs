using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleArea : MonoBehaviour
{
    [SerializeField] Transform circlePrefab;
    [SerializeField] float gap;
    [SerializeField] int battleRadius = 11;
    int gridWidth, gridHeight;
    int seatsOnRow;
    Vector2 startPos;
    Vector2 circleSize;

    public List<HexCircle> circlesPool;

    // Start is called before the first frame update
    void Start()
    {
        circlesPool = new List<HexCircle>();
        gridWidth = gridHeight = battleRadius;
        seatsOnRow = gridHeight / 2 + 1;
        AddGap();
        CalcStartPos();
        GenerateBattleArea();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void NewCircle(int x , int y, Vector2 pos)
    {
        var go = Instantiate<Transform>(circlePrefab, transform);
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
                        if ( x == gridWidth - 1)
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

    bool CalcPosition(int x, int y, out Vector2 pos , bool forcePos = false)
    {
        pos = Vector2.negativeInfinity;

        var ignoreL = (gridWidth - seatsOnRow) / 2;
        var ignoreR = seatsOnRow + ignoreL + 1;
        Debug.Log(gridWidth - seatsOnRow);
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

    public List<HexCircle> GetNearCircle(Point p)
    {
        foreach (var item in circlesPool)
        {
            item.GetComponent<SpriteRenderer>().color = Color.white;
        }
        return GetNearCircle(p.x, p.y);
    }

    public List<HexCircle> GetNearCircle(int x, int y)
    {
        var tempList = circlesPool.Where(obj=>obj.PointIndex.x == x && obj.PointIndex.y == y).ToList();

        var left = circlesPool.Find(obj => obj.PointIndex.x == x - 1 && obj.PointIndex.y == y);
        var right = circlesPool.Find(obj => obj.PointIndex.x == x + 1 && obj.PointIndex.y == y);

        if (left != null)
            tempList.Add(left);
        if (right != null)
            tempList.Add(right);

        int offset = -1;
        if (y % 2 == 0)
        {
            offset = 1;
        }

        //2 top
        var upperLeft = circlesPool.Find(obj => obj.PointIndex.x == x + offset && obj.PointIndex.y == y - 1);
        var upperRight = circlesPool.Find(obj => obj.PointIndex.x == x  && obj.PointIndex.y == y -1);

        if (upperLeft != null)
            tempList.Add(upperLeft);
        if (upperRight != null)
            tempList.Add(upperRight);

        //2 bottom
        var lowwerLeft = circlesPool.Find(obj => obj.PointIndex.x == x + offset && obj.PointIndex.y == y + 1);
        var lowwerRight = circlesPool.Find(obj => obj.PointIndex.x == x && obj.PointIndex.y == y + 1);

        if (lowwerLeft != null)
            tempList.Add(lowwerLeft);
        if (lowwerRight != null)
            tempList.Add(lowwerRight);

        return tempList;
    }

}
