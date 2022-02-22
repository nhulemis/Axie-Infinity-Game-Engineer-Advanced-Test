using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void OnEndGame(Team winner);
public delegate void OnAddActor(Character actor);
public static class CallBackService
{
    public static OnEndGame OnEndGame;
    public static OnAddActor OnAddActor;
}
