using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "game define", menuName = "new game define", order = 0)]

public class GameDefine : ScriptableObject
{
    [Header("special popup")]
    public PopupInfo popupInfo;


    [Header("Gameplay define")]
    public int teamSizeNormal;
    public int teamSizeExtra;

    [Range(10, 50)]
    public int actionCountInFrame = 20;

}
