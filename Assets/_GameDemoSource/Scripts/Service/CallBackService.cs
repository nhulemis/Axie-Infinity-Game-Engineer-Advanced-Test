using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate void OnEndGame(Team winner);
public delegate void OnAddActor(Character actor);
public delegate void OnCharacterHPChanged();
public delegate void OnCharacterTargetChanged();
public delegate void OnStartGame(Vector3 middleHexPosition);
public delegate void OnPauseGame();
public delegate void OnResume();
public delegate void OnSpeedUp();
public delegate void OnTeamMemberChanged();
public delegate void OnGiveUp();
public static class CallBackService
{
    public static OnEndGame OnEndGame;
    public static OnAddActor OnAddActor;
    public static OnCharacterHPChanged OnCharacterHPChanged;
    public static OnCharacterTargetChanged OnCharacterTargetChanged;
    public static OnStartGame OnStartGame;
    public static OnPauseGame OnPauseGame;
    public static OnResume OnResume;
    public static OnSpeedUp OnSpeedUp;
    public static OnGiveUp OnGiveUp;
    public static OnTeamMemberChanged OnTeamMemberChanged;

}
