using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupPower : MonoBehaviour
{
    [SerializeField] Slider powerBar;

    private void OnEnable()
    {
        CallBackService.OnTeamMemberChanged += OnTeamMemberChanged;
    }

    private void OnDisable()
    {
        CallBackService.OnTeamMemberChanged -= OnTeamMemberChanged;
    }

    void OnTeamMemberChanged()
    {
        int memberAtkTeam = GM.BattleArea.AttackTeam.GetMembersAlive();
        int memberDefTeam = GM.BattleArea.DefenseTeam.GetMembersAlive();

        float percentAtkTeam = (float)memberAtkTeam / GM.BattleArea.AttackTeam.GetTotalMember();
        float percentDefTeam = (float)memberDefTeam / GM.BattleArea.DefenseTeam.GetTotalMember();

        float sum = percentAtkTeam + percentDefTeam;

        float power = (float)percentAtkTeam / sum;

       // Debug.Log("Power :" + power + $" || {memberAtkTeam} - {memberDefTeam}");
        powerBar.DOValue(power, 0.5f);
    }
}
