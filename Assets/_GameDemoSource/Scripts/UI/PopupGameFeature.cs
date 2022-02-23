using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupGameFeature : Popup
{
    [SerializeField] GameObject speedx2;
    [SerializeField] TextMeshProUGUI lbSpeed;
    public void PauseGame()
    {
        CallBackService.OnPauseGame?.Invoke();
    }

    public void SpeedUp()
    {
        CallBackService.OnSpeedUp?.Invoke();
        lbSpeed.text = "Speed x" + GM.CurrentGameSpeed;
        speedx2.SetActive(GM.CurrentGameSpeed == 2);
    }

}
