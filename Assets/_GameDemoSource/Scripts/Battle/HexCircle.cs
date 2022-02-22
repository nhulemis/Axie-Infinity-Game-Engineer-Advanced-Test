using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexCircle : MonoBehaviour
{
    [SerializeField] GameObject blur;

    public Point PointIndex { get; set; }
    public Character Owner { get; set; }

    public bool IsInsideCameraView { get; set; }

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
        //var connected = GetNeighbor(1);

        //foreach (var item in connected)
        //{
        //    item. SetBlur(false);
        //}
        //GetComponent<SpriteRenderer>().color = Color.gray;
    }

    private void OnBecameInvisible()
    {
        IsInsideCameraView = false;
    }

    private void OnBecameVisible()
    {
        IsInsideCameraView = true;
    }

    internal void ClearBlur()
    {
        GM.BattleArea.RefreshCircle();

        var connected = GetNeighbor(6);

        foreach (var item in connected)
        {
            item.SetBlur(false);
        }
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    public List<HexCircle> GetNeighbor(int radius)
    {
        return GM.BattleArea.GetNeighbor(PointIndex, radius);
    }

    public Vector3 GetPosition()
    {
       return transform.position;
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