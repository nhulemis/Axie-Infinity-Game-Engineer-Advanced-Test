using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexCircle : MonoBehaviour
{
    public Point PointIndex { get; set; }
    [SerializeField] GameObject blur;

    private void OnEnable()
    {
        SetBlur(true);
    }

    public void SetBlur(bool b)
    {
        blur.SetActive(b);
    }

    public void OnPointerDown()
    {
        var battle = FindObjectOfType<BattleArea>();
        battle.RefreshCircle();
        var connected =  battle.GetNearCircle(PointIndex , 6);

        foreach (var item in connected)
        {
            item. SetBlur(false);
        }
        GetComponent<SpriteRenderer>().color = Color.gray;
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