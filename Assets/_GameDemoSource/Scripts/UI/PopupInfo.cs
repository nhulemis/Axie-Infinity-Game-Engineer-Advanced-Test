using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupInfo : Popup
{
    [SerializeField]
    RectTransform content;


    public void OpenPopup(Vector3 positionClick)
    {
        Vector2 myPositionOnScreen = Camera.main.WorldToScreenPoint(positionClick);

        Canvas copyOfMainCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        float scaleFactor = copyOfMainCanvas.scaleFactor;


        Vector2 finalPosition = new Vector2(myPositionOnScreen.x / scaleFactor, myPositionOnScreen.y / scaleFactor);

        content.anchoredPosition = finalPosition;
    }
}
