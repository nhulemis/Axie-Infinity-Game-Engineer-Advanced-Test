using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardItem : MonoBehaviour
{
    public Team team;

    [SerializeField] TextMeshProUGUI teammemberAlive;


    public void FillAlive()
    {
        int alive = 0;
        int total = 0;
        if (team == Team.Attack)
        {
            alive = GM.BattleArea.AttackTeam.GetMembersAlive();
            total = GM.BattleArea.AttackTeam.GetTotalMember();
        }
        else
        {
            alive = GM.BattleArea.DefenseTeam.GetMembersAlive();
            total = GM.BattleArea.DefenseTeam.GetTotalMember();
        }

        teammemberAlive.text = $"{alive}/{total}";
    }
}
