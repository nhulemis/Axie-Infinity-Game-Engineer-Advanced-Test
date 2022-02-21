using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexCircle : MonoBehaviour
{
    public Point PointIndex { get; set; }

    public void OnPointerDown()
    {
        var battle = FindObjectOfType<BattleArea>();

       var connected =  battle.GetNearCircle(PointIndex);

        foreach (var item in connected)
        {
            item.GetComponent<SpriteRenderer>().color = Color.red;
        }
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

[Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}