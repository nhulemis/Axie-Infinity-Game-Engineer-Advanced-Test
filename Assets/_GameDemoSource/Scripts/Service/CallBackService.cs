using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void OnEndGame(Team winner);
public static class CallBackService
{
    public static OnEndGame OnEndGame;
}
