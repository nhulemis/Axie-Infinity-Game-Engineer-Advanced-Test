using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupEndGame : PopupResult
{
    [SerializeField] Transform winnerSlot, loserSlot;
    [SerializeField] LeaderboardItem attackerBoard, defenderBoard;

    public void FillData(Team winner)
    {
        if (winner == Team.Attack)
        {
            Show(attackerBoard, defenderBoard);
        }
        else
        {
            Show(defenderBoard, attackerBoard);
        }
    }

    private void Show(LeaderboardItem winner , LeaderboardItem loser)
    {
        var win = Instantiate(winner, winnerSlot);
        win.FillAlive();
        var lose = Instantiate(loser, loserSlot);
        lose.FillAlive();
    }
}
