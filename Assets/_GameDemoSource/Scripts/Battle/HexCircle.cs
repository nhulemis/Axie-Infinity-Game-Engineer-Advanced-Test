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
        Owner?.OnPointerDown();
    }

    private void OnBecameInvisible()
    {
        IsInsideCameraView = false;
    }

    private void OnBecameVisible()
    {
        IsInsideCameraView = true;
    }

    internal void ClearBlur(Color targetColor)
    {
        GM.BattleArea.RefreshCircle();

        var connected = GetNeighbor(6);

        foreach (var item in connected)
        {
            item.SetBlur(false);
        }
        GetComponent<SpriteRenderer>().color = targetColor;
    }

    public List<HexCircle> GetNeighbor(int radius = 1)
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