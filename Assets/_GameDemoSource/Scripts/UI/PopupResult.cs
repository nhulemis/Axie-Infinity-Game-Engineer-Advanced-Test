using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupResult : Popup
{
    public void OnStartNormalGame() 
    {
        GM.StartGame(GM.Define.teamSizeNormal);
    }

    public void OnStartExtraGame()
    {
        GM.StartGame(GM.Define.teamSizeExtra);

    }
}
