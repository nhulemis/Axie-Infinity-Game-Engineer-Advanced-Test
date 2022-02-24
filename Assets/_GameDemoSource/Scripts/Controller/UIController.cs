using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class UIController : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    public T GetSpecialPopup<T>() where T : Popup
    {
        var popupPrefab = GM.Define.GET<T>();
        Assert.IsNotNull(popupPrefab, $"can't find popup type from gamedefine.asset {typeof(T).ToString()}");
        return popupPrefab as T;
    }

    internal T OpenPopup<T>() where T : Popup
    {
        var popupPrefab = GetSpecialPopup<T>();

        var popup = Instantiate(popupPrefab);
        //popup.SetActive(true);
        popup.transform.localScale = Vector3.zero;
        popup.transform.SetParent(canvas.transform, false);

        popup.GetComponent<T>().OpenPopup();
        return popup;
    }

}
