using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleTeam : MonoBehaviour
{
    [SerializeField] Team Team;
    [SerializeField] Character charPrefab;

    List<Character> teamMember;
    public List<Character> TeamMember
    {
        get => teamMember;
        private set => teamMember = value;
    }

    public void Init(BattleArea battleArea, Point middlePoint, int teamRadius, int ignoreRadius = 0)
    {
        Clear();

        // Init Defender
        teamMember = new List<Character>();

        var demesne = battleArea.GetNeighbor(middlePoint, teamRadius);

        if (ignoreRadius != 0)
        {
            var ignoreDemesne = battleArea.GetNeighbor(middlePoint, ignoreRadius);
            demesne = battleArea.GetNeighbor(middlePoint, ignoreRadius + teamRadius).Except(ignoreDemesne).ToList();
        }

        foreach (var d in demesne)
        {
            var go = Instantiate(charPrefab, transform);
            go.Init(d);
            teamMember.Add(go);
        }
    }

    private void Clear()
    {
        if (TeamMember != null)
        {
            for (int i = TeamMember.Count - 1; i >= 0; i--)
            {
                Destroy(TeamMember[i].gameObject);
            }
        }
    }

    public int GetMembersAlive()
    {
        int count = teamMember.Where(x => x.IsAlive()).Count();
        return count;
    }
    public int GetTotalMember()
    {
        return teamMember.Count();
    }
}
